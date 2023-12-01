﻿namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public record CreateProjectResponse(
    Guid ProjectId,
    Guid CompanyId,
    string ProjectName,
    string ProjectIdentifier);