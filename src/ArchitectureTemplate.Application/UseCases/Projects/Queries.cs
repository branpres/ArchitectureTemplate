namespace ArchitectureTemplate.Application.UseCases.Projects;

internal static class Queries
{
    public async static Task<bool> IsProjectNameAvailable(this IQueryable<Project> project, Guid companyId, string projectName, CancellationToken cancellationToken)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId && x.ProjectName.ToUpper() == projectName.ToUpper(), cancellationToken);
    }

    public async static Task<bool> IsProjectIdentifierAvailable(this IQueryable<Project> project, Guid companyId, string projectIdentifier, CancellationToken cancellationToken)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId && x.ProjectIdentifier != null && x.ProjectIdentifier.ToUpper() == projectIdentifier.ToUpper(), cancellationToken);
    }
}