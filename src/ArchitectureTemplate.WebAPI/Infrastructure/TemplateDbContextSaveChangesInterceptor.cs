namespace ArchitectureTemplate.WebAPI.Infrastructure;

public class TemplateDbContextSaveChangesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DomainEventDispatcher.DispatchDomainEvents((TemplateDbContext)eventData.Context!);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}