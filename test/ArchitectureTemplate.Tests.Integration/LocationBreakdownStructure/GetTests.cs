namespace ArchitectureTemplate.Tests.Integration.LocationBreakdownStructure;

public class GetTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldNotGetScopePackageIfDoesNotExist()
    {
        var httpResponse = await _httpClient.GetAsync($"/scopepackage?projectId={Guid.NewGuid()}");
        var response = await httpResponse.Content.ReadFromJsonAsync<List<GetScopePackagesByProjectIdResponse>>();

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Empty(response!);
    }
}