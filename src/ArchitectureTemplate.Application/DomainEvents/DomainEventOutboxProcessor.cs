namespace ArchitectureTemplate.Application.DomainEvents;

public class DomainEventOutboxProcessor(IServiceScopeFactory serviceScopeFactory, ILogger<DomainEventOutboxProcessor> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger<DomainEventOutboxProcessor> _logger = logger;

    private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(10));

    private const int MAX_NUMBER_OF_RETRIES = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var templateDbContext = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();

            var notProcessedOutboxMessagesQuery = templateDbContext.OutboxMessage
                .Include(x => x.OutboxMessageHandlerInstances.Where(x => x.OutboxMessageHandlerInstanceStatus != OutboxMessageHandlerInstanceStatus.Succeeded))
                .Where(x => (x.OutboxMessageStatus == OutboxMessageStatus.NotProcessed || x.OutboxMessageStatus == OutboxMessageStatus.ProcessingFailed)
                    && x.NumberOfRetries < MAX_NUMBER_OF_RETRIES);
            var notProcessedOutboxMessages = await notProcessedOutboxMessagesQuery.ToListAsync(stoppingToken);

            await notProcessedOutboxMessagesQuery.ExecuteUpdateAsync(x => x.SetProperty(y => y.OutboxMessageStatus, OutboxMessageStatus.Processing), stoppingToken);

            foreach (var outboxMessage in notProcessedOutboxMessages)
            {
                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content);
                    if (domainEvent != null)
                    {
                        var domainEventOutboxMessageHandlerType = typeof(IDomainEventOutboxMessageHandler<>).MakeGenericType(domainEvent.GetType());
                        var handleMethod = domainEventOutboxMessageHandlerType.GetMethod("Handle");
                        var domainEventOutboxMessageHandlers = scope.ServiceProvider.GetServices(domainEventOutboxMessageHandlerType);
                        if (domainEventOutboxMessageHandlers != null && domainEventOutboxMessageHandlers.Any())
                        {
                            foreach (var domainEventOutboxMessageHandler in domainEventOutboxMessageHandlers)
                            {
                                if (domainEventOutboxMessageHandler != null)
                                {
                                    var handlerName = domainEventOutboxMessageHandler.GetType().Name;

                                    var outboxMessageHandlerInstance = outboxMessage.OutboxMessageHandlerInstances.FirstOrDefault(x => x.HandlerName == handlerName);
                                    if (outboxMessageHandlerInstance == null)
                                    {
                                        outboxMessageHandlerInstance = new OutboxMessageHandlerInstance(outboxMessage, handlerName);
                                        outboxMessage.AddOutboxMessageHandlerInstance(outboxMessageHandlerInstance);
                                    }

                                    try
                                    {
                                        await handleMethod?.Invoke((dynamic)domainEventOutboxMessageHandler, new object[] { domainEvent });
                                        outboxMessageHandlerInstance.MarkSucceeded();
                                    }
                                    catch (Exception ex)
                                    {
                                        outboxMessageHandlerInstance.MarkFailed();

                                        _logger.LogError(ex, "Exception occurred while processing outbox message.");
                                    }
                                }
                            }
                        }
                    }

                    var outboxMessageStatus = outboxMessage.OutboxMessageHandlerInstances
                        .All(x => x.OutboxMessageHandlerInstanceStatus == OutboxMessageHandlerInstanceStatus.Succeeded)
                            ? OutboxMessageStatus.ProcessingComplete
                            : OutboxMessageStatus.ProcessingFailed;

                    await templateDbContext.OutboxMessage.Where(x => x.OutboxMessageId == outboxMessage.OutboxMessageId)
                        .ExecuteUpdateAsync(x => x.SetProperty(y => y.OutboxMessageStatus, outboxMessageStatus), stoppingToken);
                }
                catch (Exception ex)
                {
                    await templateDbContext.OutboxMessage.Where(x => x.OutboxMessageId == outboxMessage.OutboxMessageId)
                        .ExecuteUpdateAsync(x => x.SetProperty(y => y.OutboxMessageStatus, OutboxMessageStatus.ProcessingFailed), stoppingToken);

                    _logger.LogError(ex, "Exception occurred while processing outbox message.");
                }
            }

            await templateDbContext.SaveChangesAsync(stoppingToken);
        }
    }
}