namespace ArchitectureTemplate.Application.UseCases.Projects;

internal static class Queries
{
    public async static Task<bool> IsProjectNameAvailable(this DbSet<Project> project, Guid companyId, string projectName)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId
                && x.ProjectName.ToUpper() == projectName.ToUpper());
    }

    public async static Task<bool> IsProjectIdentifierAvailable(this DbSet<Project> project, Guid companyId, string projectIdentifier)
    {
        return !await project
            .AsNoTracking()
            .AnyAsync(x => x.CompanyId == companyId
                && x.ProjectIdentifier != null
                && x.ProjectIdentifier.ToUpper() == projectIdentifier.ToUpper());
    }
}