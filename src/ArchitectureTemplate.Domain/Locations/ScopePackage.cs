namespace ArchitectureTemplate.Domain.Locations;

public class ScopePackage
{
    public Guid ScopePackageId { get; set; }

    public Guid ProjectId { get; set; }

    public Project? Project { get; set; }

    public string ScopePackageName { get; set; } = "Scope Package 1";
}