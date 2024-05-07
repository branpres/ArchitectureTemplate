namespace ArchitectureTemplate.WebAPI.Infrastructure;

public class TemplateDbContextSaveChangesInterceptor(DomainEventDispatcher domainEventDispatcher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await domainEventDispatcher.DispatchDomainEvents((TemplateDbContext)eventData.Context!);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}