namespace ArchitectureTemplate.Tests.Integration.Projects;

public class DeleteTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldDeleteProject()
    {
        var endpointResponse = await CreateProjectAndGetEndpointResponse(new CreateProjectRequest(Guid.NewGuid(), "Test"));
        var deleteResponse = await _httpClient.DeleteAsync(endpointResponse!.Links!.First(x => x.Name == "DeleteProject").Href);

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse?.StatusCode);

        // should not be able to retrieve project after it is deleted
        var getResponse = await _httpClient.GetAsync(endpointResponse!.Links!.First(x => x.Name == "GetProjectById").Href);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task ShouldNotDeleteProjectIfDoesNotExist()
    {
        var response = await _httpClient.DeleteAsync($"/project/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}