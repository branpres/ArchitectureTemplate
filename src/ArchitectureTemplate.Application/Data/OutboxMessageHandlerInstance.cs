namespace ArchitectureTemplate.Application.Data;

public class OutboxMessageHandlerInstance: IBasicMetadata
{
    private OutboxMessageHandlerInstance() { }

    private OutboxMessageHandlerInstance(OutboxMessage outboxMessage, string handlerName)
    {
        OutboxMessage = outboxMessage;
        HandlerName = handlerName;
    }

    public Guid OutboxMessageHandlerInstanceId { get; private set; }

    public Guid OutboxMessageId { get; private set; }

    public OutboxMessage? OutboxMessage { get; }

    public string? HandlerName { get; private set; }

    public OutboxMessageHandlerInstanceStatus OutboxMessageHandlerInstanceStatus { get; private set; } = OutboxMessageHandlerInstanceStatus.NotExecuted;

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public static OutboxMessageHandlerInstance CreateInstance(OutboxMessage outboxMessage, string handlerName) => new(outboxMessage, handlerName);

    public void MarkSucceeded()
    {
        OutboxMessageHandlerInstanceStatus = OutboxMessageHandlerInstanceStatus.Succeeded;
    }

    public void MarkFailed()
    {
        OutboxMessageHandlerInstanceStatus = OutboxMessageHandlerInstanceStatus.Failed;
    }
}

public enum OutboxMessageHandlerInstanceStatus
{
    NotExecuted,
    Succeeded,
    Failed
}