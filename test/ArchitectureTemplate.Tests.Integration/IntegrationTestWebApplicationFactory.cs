namespace ArchitectureTemplate.Tests.Integration;

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Using MySQL for integration tests as Respawn does not work with Sqlite.
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder().Build();

    private DbConnection? _dbConnection;

    private Respawner? _respawner;
    
    public HttpClient? HttpClient { get; private set; }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
        _dbConnection = new MySqlConnection(_mySqlContainer.GetConnectionString());
        HttpClient = CreateClient();
        await InitializeRespawnerAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner!.ResetAsync(_dbConnection!);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthHandler>("Test", options => { });

            var serviceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TemplateDbContext>));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }
            services.AddDbContext<TemplateDbContext>(
                (sp, options) =>
                {
                    options
                        .UseMySql(
                            _mySqlContainer.GetConnectionString(),
                            ServerVersion.AutoDetect(_mySqlContainer.GetConnectionString()));
                    options.AddInterceptors(sp.GetRequiredService<TemplateDbContextSaveChangesInterceptor>());
                }
            );
        });
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection!.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection!, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql,

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