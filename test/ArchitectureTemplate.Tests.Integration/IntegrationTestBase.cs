namespace ArchitectureTemplate.Tests.Integration;

[Collection("TestCollection")]
public abstract class IntegrationTestBase(IntegrationTestWebApplicationFactory webApplicationFactory) : IAsyncLifetime
{
    protected HttpClient _httpClient = webApplicationFactory.HttpClient!;

    private readonly Func<Task> _resetDatabase = webApplicationFactory.ResetDatabaseAsync;

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}