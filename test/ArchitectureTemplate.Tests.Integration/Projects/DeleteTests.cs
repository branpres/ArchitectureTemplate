namespace ArchitectureTemplate.Tests.Integration.Projects;

public class DeleteTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldDeleteProject()
    {
        var createProjectResponse = await CreateProjectAndGetResponse(new CreateProjectRequest(Guid.NewGuid(), "Test"));
        var deleteResponse = await _httpClient.DeleteAsync(createProjectResponse!.Links!.First(x => x.Name == "DeleteProject").Href);

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // should not be able to retrieve project after it is deleted
        var getResponse = await _httpClient.GetAsync(createProjectResponse.Links!.First(x => x.Name == "GetProjectById").Href);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task ShouldDeleteProjectAndBillOfMaterials()
    {
        var createProjectResponse = await CreateProjectAndGetResponse(new CreateProjectRequest(Guid.NewGuid(), "Test"));
        await _httpClient.DeleteAsync(createProjectResponse!.Links!.First(x => x.Name == "DeleteProject").Href);

        // should not be able to retrieve bill of materials as deleting the project also deletes it
        var getResponse = await _httpClient.GetAsync(createProjectResponse.Links!.First(x => x.Name == "GetBillOfMaterialsByProjectId").Href);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task ShouldDeleteProjectAndScopePackage()
    {
        var createProjectResponse = await CreateProjectAndGetResponse(new CreateProjectRequest(Guid.NewGuid(), "Test"));
        await _httpClient.DeleteAsync(createProjectResponse!.Links!.First(x => x.Name == "DeleteProject").Href);

        // should retrieve no scope packages as deleting the project also deletes them all
        var getResponse = await _httpClient.GetAsync(createProjectResponse.Links!.First(x => x.Name == "GetScopePackagesByProjectId").Href);
        var scopePackageResponse = await getResponse.Content.ReadFromJsonAsync<List<GetScopePackagesByProjectIdResponse>>();
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.Empty(scopePackageResponse!);
    }

    [Fact]
    public async Task ShouldNotDeleteProjectIfDoesNotExist()
    {
        var response = await _httpClient.DeleteAsync($"/project/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}