using ArchitectureTemplate.Domain.DomainEvents;
using ArchitectureTemplate.Domain.Interfaces;

namespace ArchitectureTemplate.Application.Data;

public class TemplateDbContext(DbContextOptions options, IDomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
    private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public DbSet<Project> Project { get; set; }

    public DbSet<ProjectUser> ProjectUser {  get; set; }

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
                    entry.Entity.CreatedBy = Guid.NewGuid(); // pretending we have a user
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = Guid.NewGuid(); // pretending we have a user
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
            }
        }

        await _domainEventDispatcher.DispatchDomainEvents(GetRegisteredDomainEvents(this));

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private static List<DomainEventEntityBase> GetRegisteredDomainEvents(DbContext dbContext)
    {
        return dbContext.ChangeTracker.Entries<DomainEventEntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();
    }
}