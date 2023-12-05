namespace ArchitectureTemplate.Application.Data;

public class OutboxMessage(string type, string content) : IBasicMetadata
{
    public Guid OutboxMessageId { get; private set; }

    public string Type { get; private set; } = type;

    public string Content { get; private set; } = content;

    public OutboxMessageStatus OutboxMessageStatus { get; private set; } = OutboxMessageStatus.NotProcessed;

    public int NumberOfRetries { get; private set; }

    public List<OutboxMessageHandlerInstance> OutboxMessageHandlerInstances { get; private set; } = [];

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public void AddOutboxMessageHandlerInstance(OutboxMessageHandlerInstance instance)
    {
        OutboxMessageHandlerInstances.Add(instance);
    }
}

public enum OutboxMessageStatus
{
    NotProcessed,
    Processing,
    ProcessingComplete,
    ProcessingFailed
}