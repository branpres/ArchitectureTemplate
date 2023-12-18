namespace ArchitectureTemplate.Tests.Integration;

[Collection("TestCollection")]
public abstract class IntegrationTestBase(IntegrationTestWebApplicationFactory webApplicationFactory) : IAsyncLifetime
{
    protected HttpClient _httpClient = webApplicationFactory.HttpClient!;

    private readonly Func<Task> _resetDatabase = webApplicationFactory.ResetDatabaseAsync;

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    protected async Task<HttpResponseMessage> CreateProject(CreateProjectRequest createProjectRequest)
    {
        return await _httpClient.PostAsJsonAsync("/project", createProjectRequest);
    }

    protected async Task<CreateProjectResponse?> CreateProjectAndGetResponse(CreateProjectRequest createProjectRequest)
    {
        var createResponse = await CreateProject(createProjectRequest);
        return await createResponse.Content.ReadFromJsonAsync<CreateProjectResponse>();
    }

    protected async Task<HttpResponseMessage> CreateProjectAndGetItBack(CreateProjectRequest createProjectRequest)
    {
        var createResponse = await CreateProjectAndGetResponse(createProjectRequest);
        var getProjectByIdLink = createResponse!.Links!.First(x => x.Name == "GetProjectById").Href;

        return await _httpClient.GetAsync(getProjectByIdLink);
    }
}