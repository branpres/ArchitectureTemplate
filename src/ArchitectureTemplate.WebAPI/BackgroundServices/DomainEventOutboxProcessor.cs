﻿namespace ArchitectureTemplate.WebAPI.BackgroundServices;

public class DomainEventOutboxProcessor(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<DomainEventOutboxProcessor> logger,
    IConfiguration configuration) : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(10));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var maxNumberOfTries = int.Parse(configuration["DomainEventOutboxProcessor:MaxNumberOfTries"]!);

        while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = serviceScopeFactory.CreateScope();

            var templateDbContext = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();

            var notProcessedOutboxMessagesQuery = templateDbContext.OutboxMessage
                .Include(x => x.OutboxMessageHandlerInstances)
                .Where(x => (x.OutboxMessageStatus == OutboxMessageStatus.NotProcessed || x.OutboxMessageStatus == OutboxMessageStatus.ProcessingFailed)
                    && x.NumberOfTries <= maxNumberOfTries);
            var notProcessedOutboxMessages = await notProcessedOutboxMessagesQuery.ToListAsync(stoppingToken);

            await notProcessedOutboxMessagesQuery.ExecuteUpdateAsync(x => x
                .SetProperty(y => y.OutboxMessageStatus, OutboxMessageStatus.Processing)
                .SetProperty(y => y.UpdatedOn, DateTime.UtcNow),
                stoppingToken);

            var handlers = GetHandlers();

            foreach (var outboxMessage in notProcessedOutboxMessages)
            {
                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                    if (domainEvent != null)
                    {
                        if (handlers.TryGetValue(domainEvent.GetType(), out var outboxMessagehandlers))
                        {
                            foreach (var handler in outboxMessagehandlers)
                            {
                                var handlerName = handler.GetType().Name;

                                var outboxMessageHandlerInstance = outboxMessage.OutboxMessageHandlerInstances.FirstOrDefault(x => x.HandlerName == handlerName);
                                if (outboxMessageHandlerInstance == null)
                                {
                                    outboxMessageHandlerInstance = new OutboxMessageHandlerInstance(outboxMessage.OutboxMessageId, handlerName);
                                    outboxMessage.AddOutboxMessageHandlerInstance(outboxMessageHandlerInstance);
                                }

                                try
                                {
                                    if (outboxMessageHandlerInstance.OutboxMessageHandlerInstanceStatus != OutboxMessageHandlerInstanceStatus.Succeeded)
                                    {
                                        await handler.Handle(domainEvent);
                                        outboxMessageHandlerInstance.MarkSucceeded();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    outboxMessageHandlerInstance.MarkFailed();

                                    logger.LogError(ex, "Exception occurred when invoking outbox message handler.");
                                }
                            }
                        }                                                 
                    }

                    var outboxMessageStatus = outboxMessage.OutboxMessageHandlerInstances
                        .TrueForAll(x => x.OutboxMessageHandlerInstanceStatus == OutboxMessageHandlerInstanceStatus.Succeeded)
                            ? OutboxMessageStatus.ProcessingComplete
                            : OutboxMessageStatus.ProcessingFailed;

                    await templateDbContext.OutboxMessage.Where(x => x.OutboxMessageId == outboxMessage.OutboxMessageId)
                        .ExecuteUpdateAsync(x => x
                            .SetProperty(y => y.OutboxMessageStatus, outboxMessageStatus)
                            .SetProperty(y => y.NumberOfTries, outboxMessage.NumberOfTries + 1)
                            .SetProperty(y => y.UpdatedOn, DateTime.UtcNow),
                            stoppingToken);
                }
                catch (Exception ex)
                {
                    await templateDbContext.OutboxMessage.Where(x => x.OutboxMessageId == outboxMessage.OutboxMessageId)
                        .ExecuteUpdateAsync(x => x
                            .SetProperty(y => y.OutboxMessageStatus, OutboxMessageStatus.ProcessingFailed)
                            .SetProperty(y => y.NumberOfTries, outboxMessage.NumberOfTries + 1)
                            .SetProperty(y => y.UpdatedOn, DateTime.UtcNow),
                            stoppingToken);

                    logger.LogError(ex, "Exception occurred when processing outbox message.");
                }
            }

            if (notProcessedOutboxMessages.Count > 0)
            {
                await templateDbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }

    private static Dictionary<Type, List<IDomainEventOutboxMessageHandler>> GetHandlers()
    {
        return new Dictionary<Type, List<IDomainEventOutboxMessageHandler>>
        {
            {
                typeof(ProjectUserAdded),
                [
                    new WhenProjectUserAddedSendEmail()
                ]
            },
            {
                typeof(ProjectAdminUserAdded),
                [
                    new WhenProjectAdminUserAddedSendEmail()
                ]
            }
        };
    }
}