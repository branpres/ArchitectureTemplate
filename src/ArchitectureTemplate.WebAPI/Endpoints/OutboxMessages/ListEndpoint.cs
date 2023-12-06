namespace ArchitectureTemplate.WebAPI.Endpoints.OutboxMessages;

public class ListEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/outbox", List)
            .WithOpenApi(x => new(x)
            {
                OperationId = "ListOutboxMessages",
                Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Outbox" } },
                Description = "Lists all outbox messages."
            })
            .Produces(StatusCodes.Status200OK);

        return builder;
    }

    private async Task<IResult> List(TemplateDbContext templateDbContext, CancellationToken cancellationToken)
    {
        var outboxMessages = await templateDbContext.OutboxMessage
            .Include(x => x.OutboxMessageHandlerInstances)
            .Select(x => new OutboxMessageResponse
            {
                OutboxMessageId = x.OutboxMessageId,
                Type = x.Type,
                Content = x.Content,
                OutboxMessageStatus = x.OutboxMessageStatus.ToString(),
                NumberOfTries = x.NumberOfTries,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedOn = x.UpdatedOn,
                UpdatedBy = x.UpdatedBy,
                HandlerInstances = x.OutboxMessageHandlerInstances
                    .Select(y => new OutboxMessageHandlerInstanceResponse
                    {
                        OutboxMessageHandlerInstanceId = y.OutboxMessageHandlerInstanceId,
                        HandlerName = y.HandlerName,
                        OutboxMessageHandlerInstanceStatus = y.OutboxMessageHandlerInstanceStatus.ToString(),
                        CreatedOn = y.CreatedOn,
                        CreatedBy = y.CreatedBy,
                        UpdatedOn = y.UpdatedOn,
                        UpdatedBy = y.UpdatedBy
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return Results.Ok(outboxMessages);
    }
}

public class OutboxMessageResponse
{
    public Guid OutboxMessageId { get; set; }

    public string? Type { get; set; }

    public string? Content { get; set; }

    public string? OutboxMessageStatus { get; set; }

    public int NumberOfTries { get; set; }

    public List<OutboxMessageHandlerInstanceResponse>? HandlerInstances { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}

public class OutboxMessageHandlerInstanceResponse
{
    public Guid OutboxMessageHandlerInstanceId { get; set; }

    public string? HandlerName { get; set; }

    public string? OutboxMessageHandlerInstanceStatus { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}