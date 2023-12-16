global using ArchitectureTemplate.WebAPI.Domain.Aggregates.LocationBreakdownStructure;
global using ArchitectureTemplate.WebAPI.Endpoints;
global using ArchitectureTemplate.WebAPI.Features.BOMs.GetByProjectId;
global using ArchitectureTemplate.WebAPI.Features.LocationBreakdownStructure.GetScopePackagesByProjectId;
global using ArchitectureTemplate.WebAPI.Features.Projects.Create;
global using ArchitectureTemplate.WebAPI.Features.Projects.GetById;
global using ArchitectureTemplate.WebAPI.Infrastructure;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using MySqlConnector;
global using Respawn;
global using System.Data.Common;
global using System.Net;
global using System.Net.Http.Json;
global using System.Security.Claims;
global using System.Text.Encodings.Web;
global using Testcontainers.MySql;
global using Xunit;