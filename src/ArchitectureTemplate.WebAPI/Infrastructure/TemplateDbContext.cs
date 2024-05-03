namespace ArchitectureTemplate.WebAPI.Infrastructure;

public class TemplateDbContext(
    DbContextOptions options,
    ICurrentUser currentUser,
    DomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
    public DbSet<Project> Project { get; set; }

    public DbSet<ProjectUser> ProjectUser {  get; set; }

    public DbSet<BillOfMaterials> BillOfMaterials { get; set; }

    public DbSet<ScopePackage> ScopePackage { get; set; }

    public DbSet<OutboxMessage> OutboxMessage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await domainEventDispatcher.DispatchDomainEvents();

        foreach (var entry in ChangeTracker.Entries<IBasicMetadata>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    if (currentUser.User != null)
                    {
                        entry.Entity.CreatedBy = currentUser.User.UserId;
                    }                    
                    
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    if (currentUser.User != null)
                    {
                        entry.Entity.UpdatedBy = currentUser.User.UserId;
                    }
                    
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<IDeleteMetadata>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    if (entry.Entity.IsDeleted)
                    {
                        entry.Entity.DeletedOn = DateTime.UtcNow;
                        if (currentUser.User != null)
                        {
                            entry.Entity.DeletedBy = currentUser.User.UserId;
                        }
                    }
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}