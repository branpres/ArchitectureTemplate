namespace ArchitectureTemplate.WebAPI.Shared;

public static class ProjectQueries
{
    public async static Task<bool> IsProjectNameAvailable(this IQueryable<Project> project, Guid companyId, string projectName, CancellationToken cancellationToken)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId && x.ProjectName!.ToUpper() == projectName.ToUpper(), cancellationToken);
    }

    public async static Task<bool> IsProjectIdentifierAvailable(this IQueryable<Project> project, Guid companyId, string projectIdentifier, CancellationToken cancellationToken)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId && x.ProjectIdentifier != null && x.ProjectIdentifier.ToUpper() == projectIdentifier.ToUpper(), cancellationToken);
    }

    public async static Task<Project?> GetProjectWithProjectUsers(this IQueryable<Project> project, Guid projectId, CancellationToken cancellationToken)
    {
        return await project
            .Include(x => x.ProjectUsers.Where(y => !y.IsDeleted))
            .SingleOrDefaultAsync(x => x.ProjectId == projectId && !x.IsDeleted, cancellationToken);
    }
}