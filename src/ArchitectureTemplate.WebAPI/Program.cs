using ArchitectureTemplate.WebAPI.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
    .AddSwaggerGen()
    .AddEndpointsApiExplorer()
    .AddExceptionHandler<ExceptionHandler>()
    .AddHttpContextAccessor()
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddScoped<ICurrentUser, CurrentUser>()
    .AddHostedService<DomainEventOutboxProcessor>()
    .AddDomainEventHandling()
    .AddScoped<TemplateDbContextSaveChangesInterceptor>()
    .AddDbContext<TemplateDbContext>((sp, options) =>
    {
        var connection = new SqliteConnection("Data Source=TemplateDB;Mode=Memory;Cache=Shared");
        connection.Open();
        options.UseSqlite(connection);
        options.EnableSensitiveDataLogging();
        options.AddInterceptors(sp.GetRequiredService<TemplateDbContextSaveChangesInterceptor>());
    })
    .AddTransient<CurrentUserMiddleware>();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

var app = builder.Build();

app.UseExceptionHandler(options => { });
app.UseMiddleware<CurrentUserMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

using var scope = app.Services.CreateScope();
using var dbContext = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();
await dbContext.Database.EnsureCreatedAsync();

app.Run();

public partial class Program { }

internal static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = DiscoverEndpoints();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
        return app;
    }

    private static IEnumerable<IEndpoint> DiscoverEndpoints()
    {
        return typeof(IEndpoint).Assembly
        .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();
    }
}