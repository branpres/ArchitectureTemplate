namespace ArchitectureTemplate.WebAPI.Infrastructure.EntityTypeConfigurations;

internal class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.OutboxMessageId);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.OutboxMessageStatus)
            .IsRequired();

        builder.HasMany(x => x.OutboxMessageHandlerInstances)
            .WithOne(x => x.OutboxMessage);
    }
}