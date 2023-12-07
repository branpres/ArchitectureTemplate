namespace ArchitectureTemplate.Tests.Integration.Projects;

public class CreateTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldCreateProjectWithNoAdminUser()
    {
        var request = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var response = await _httpClient.PostAsJsonAsync("/project", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}