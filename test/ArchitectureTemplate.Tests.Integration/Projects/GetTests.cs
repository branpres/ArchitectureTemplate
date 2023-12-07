namespace ArchitectureTemplate.Tests.Integration.Projects;

public class GetTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldNotGetProjectIfDoesNotExist()
    {
        var response = await _httpClient.GetAsync($"/project/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode );
    }
}