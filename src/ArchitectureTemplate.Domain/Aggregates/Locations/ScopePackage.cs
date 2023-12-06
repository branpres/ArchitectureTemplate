﻿namespace ArchitectureTemplate.Domain.Aggregates.Locations;

public class ScopePackage : IBasicMetadata, IDeleteMetadata
{
    public Guid ScopePackageId { get; set; }

    public Guid ProjectId { get; set; }

    public Project? Project { get; set; }

    public string ScopePackageName { get; set; } = "Scope Package 1";

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}