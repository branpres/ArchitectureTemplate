namespace ArchitectureTemplate.WebAPI.Shared;

public static class ProjectQueries
{
    public static async Task<bool> IsProjectNameAvailable(this IQueryable<Project> project, Guid companyId, string projectName, CancellationToken cancellationToken)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId && x.ProjectName!.ToUpper() == projectName.ToUpper(), cancellationToken);
    }

    public static async Task<bool> IsProjectIdentifierAvailable(this IQueryable<Project> project, Guid companyId, string projectIdentifier, CancellationToken cancellationToken)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId && x.ProjectIdentifier != null && x.ProjectIdentifier.ToUpper() == projectIdentifier.ToUpper(), cancellationToken);
    }

    public static async Task<Project?> GetProjectWithProjectUsers(this IQueryable<Project> project, Guid projectId, CancellationToken cancellationToken)
    {
        return await project
            .Include(x => x.ProjectUsers.Where(y => !y.IsDeleted))
            .SingleOrDefaultAsync(x => x.ProjectId == projectId && !x.IsDeleted, cancellationToken);
    }
}