namespace ArchitectureTemplate.Tests.Integration.LocationBreakdownStructure;

public class GetTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldNotGetScopePackageIfDoesNotExist()
    {
        var response = await _httpClient.GetAsync($"/scopepackage/{Guid.NewGuid()}");
        var endpointResponse = await response.Content.ReadFromJsonAsync<EndpointResponse<List<GetScopePackagesByProjectIdResponse>>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty(endpointResponse!.Response);
    }
}