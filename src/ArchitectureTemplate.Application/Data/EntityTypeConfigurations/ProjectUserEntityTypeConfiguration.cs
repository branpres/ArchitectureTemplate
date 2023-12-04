namespace ArchitectureTemplate.Application.Data.EntityTypeConfigurations;

internal class ProjectUserEntityTypeConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.HasKey(x => x.ProjectUserId);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.Project)
            .WithMany(x => x.ProjectUsers);
    }
}