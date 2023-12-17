﻿global using ArchitectureTemplate.WebAPI.Domain.Aggregates;
global using ArchitectureTemplate.WebAPI.Domain.Aggregates.BOM;
global using ArchitectureTemplate.WebAPI.Domain.Aggregates.LocationBreakdownStructure;
global using ArchitectureTemplate.WebAPI.Domain.Aggregates.Outbox;
global using ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects;
global using ArchitectureTemplate.WebAPI.Domain.DomainEvents;
global using ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectAdminUserAdded;
global using ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectCreated;
global using ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectDeleted;
global using ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectUserAdded;
global using ArchitectureTemplate.WebAPI.Features;
global using ArchitectureTemplate.WebAPI.Infrastructure;
global using ArchitectureTemplate.WebAPI.Middleware;
global using ArchitectureTemplate.WebAPI.Shared;
global using FluentValidation;
global using FluentValidation.Results;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.Data.Sqlite;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.OpenApi.Models;
global using Newtonsoft.Json;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics;
global using System.Net;