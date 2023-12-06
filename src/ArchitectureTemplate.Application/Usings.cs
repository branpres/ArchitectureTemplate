﻿global using ArchitectureTemplate.Application.Data;
global using ArchitectureTemplate.Application.DomainEvents;
global using ArchitectureTemplate.Application.UseCases.BOMs.GetBillOfMaterialsByProjectId;
global using ArchitectureTemplate.Application.UseCases.BOMs.ResponseMappers;
global using ArchitectureTemplate.Application.UseCases.LocationBreakdownStructure.GetScopePackagesByProjectId;
global using ArchitectureTemplate.Application.UseCases.LocationBreakdownStructure.ResponseMappers;
global using ArchitectureTemplate.Application.UseCases.Projects.Create;
global using ArchitectureTemplate.Application.UseCases.Projects.CreateUser;
global using ArchitectureTemplate.Application.UseCases.Projects.GetById;
global using ArchitectureTemplate.Application.UseCases.Projects.ResponseMappers;
global using ArchitectureTemplate.Domain.Aggregates.BOMs;
global using ArchitectureTemplate.Domain.Aggregates.LocationBreakdownStructure;
global using ArchitectureTemplate.Domain.Aggregates.Projects;
global using ArchitectureTemplate.Domain.Aggregates.Projects.DomainEvents;
global using ArchitectureTemplate.Domain.Common.DomainEvents;
global using ArchitectureTemplate.Domain.Common.Interfaces;
global using ArchitectureTemplate.Domain.Common.Result;
global using FluentValidation;
global using Microsoft.AspNetCore.Http;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Newtonsoft.Json;