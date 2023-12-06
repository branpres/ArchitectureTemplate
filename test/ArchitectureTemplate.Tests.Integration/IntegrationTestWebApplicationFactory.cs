namespace ArchitectureTemplate.Tests.Integration;

internal class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient? HttpClient { get; private set; }

    public Task InitializeAsync()
    {
        HttpClient = CreateClient();

        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
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
                var connection = new SqliteConnection("Data Source=IntegrationTestTemplateDB;Mode=Memory;Cache=Shared");
                connection.Open();
                options.UseSqlite(connection);
            });

            await InitializeDatabase(services);
        });
    }

    private static async Task InitializeDatabase(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
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