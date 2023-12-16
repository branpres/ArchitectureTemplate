namespace ArchitectureTemplate.WebAPI.Infrastructure;

public class TemplateDbContext(
    DbContextOptions options,
    ICurrentUser currentUser,
    DomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
    private readonly ICurrentUser _currentUser = currentUser;

    private readonly DomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    internal DbSet<Project> Project { get; set; }

    internal DbSet<ProjectUser> ProjectUser {  get; set; }

    internal DbSet<BillOfMaterials> BillOfMaterials { get; set; }

    internal DbSet<ScopePackage> ScopePackage { get; set; }

    public DbSet<OutboxMessage> OutboxMessage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _domainEventDispatcher.DispatchDomainEvents(this);

        foreach (var entry in ChangeTracker.Entries<IBasicMetadata>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    if (_currentUser.User != null)
                    {
                        entry.Entity.CreatedBy = _currentUser.User.UserId;
                    }                    
                    
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    if (_currentUser.User != null)
                    {
                        entry.Entity.UpdatedBy = _currentUser.User.UserId;
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
                        if (_currentUser.User != null)
                        {
                            entry.Entity.DeletedBy = _currentUser.User.UserId;
                        }
                    }
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}