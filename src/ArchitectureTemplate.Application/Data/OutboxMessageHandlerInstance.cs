namespace ArchitectureTemplate.Application.Data;

public class OutboxMessageHandlerInstance(OutboxMessage outboxMessage, string handlerName) : IBasicMetadata
{
    public Guid OutboxMessageHandlerInstanceId { get; private set; }

    public Guid OutboxMessageId { get; private set; }

    public OutboxMessage? OutboxMessage { get; } = outboxMessage;

    public string HandlerName { get; private set; } = handlerName;

    public OutboxMessageHandlerInstanceStatus OutboxMessageHandlerInstanceStatus { get; private set; } = OutboxMessageHandlerInstanceStatus.NotExecuted;

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

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