namespace ArchitectureTemplate.Application.Data;

public class TemplateDbContext(
    DbContextOptions options,
    ICurrentUser currentUser,
    IDomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
    private readonly ICurrentUser _currentUser = currentUser;

    private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public DbSet<Project> Project { get; set; }

    public DbSet<ProjectUser> ProjectUser {  get; set; }

    public DbSet<BillOfMaterials> BillOfMaterials { get; set; }

    public DbSet<ScopePackage> ScopePackage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<IBasicMetadata>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUser.UserId;
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUser.UserId;
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
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
                        entry.Entity.DeletedBy = _currentUser.UserId;
                        entry.Entity.DeletedOn = DateTime.UtcNow;
                    }
                    break;
            }
        }

        await _domainEventDispatcher.DispatchDomainEvents(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}