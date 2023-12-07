namespace ArchitectureTemplate.Tests.Integration;

internal class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private SqliteConnection? _dbConnection;

    private Respawner? _respawner;
    
    public HttpClient? HttpClient { get; private set; }

    public async Task InitializeAsync()
    {
        _dbConnection = new("Data Source=IntegrationTestTemplateDB;Mode=Memory;Cache=Shared");
        HttpClient = CreateClient();
        await InitializeRespawnerAsync();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner!.ResetAsync(_dbConnection!);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(async services =>
        {
            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthHandler>("Test", options => { });

            var serviceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TemplateDbContext>));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }
            services.AddDbContext<TemplateDbContext>(options =>
            {
                options.UseSqlite(_dbConnection!);
            });

            await InitializeDatabase(services);
        });
    }

    private async Task InitializeDatabase(IServiceCollection services)
    {
        await _dbConnection!.OpenAsync();

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(_dbConnection!, new RespawnerOptions
        {
            // tables in your database that you wish to not have Respawner blow away each time it resets the database between tests
            TablesToIgnore =
            [
                //"__EFMigrationsHistory",                
            ]
        });
    }
}

public class IntegrationTestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim("cognito:username", "Test User") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");
        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}