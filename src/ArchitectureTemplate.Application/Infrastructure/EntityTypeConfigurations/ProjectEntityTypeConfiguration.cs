namespace ArchitectureTemplate.Application.Infrastructure.EntityTypeConfigurations;

internal class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.ProjectId);

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.ProjectName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.ProjectIdentifier)
            .HasMaxLength(64);

        builder.HasMany(x => x.ProjectUsers)
            .WithOne(x => x.Project);
    }
}