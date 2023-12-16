namespace ArchitectureTemplate.WebAPI.Infrastructure.EntityTypeConfigurations;

internal class OutboxMessageHandlerInstanceEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessageHandlerInstance>
{
    public void Configure(EntityTypeBuilder<OutboxMessageHandlerInstance> builder)
    {
        builder.HasKey(x => x.OutboxMessageHandlerInstanceId);

        builder.HasOne(x => x.OutboxMessage)
            .WithMany(x => x.OutboxMessageHandlerInstances);

        builder.Property(x => x.HandlerName)
            .IsRequired();

        builder.Property(x => x.OutboxMessageHandlerInstanceStatus)
            .IsRequired();
    }
}