namespace ArchitectureTemplate.Tests.Integration.BOMs;

public class GetTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldNotGetBillOfMaterialsIfDoesNotExist()
    {
        var response = await _httpClient.GetAsync($"/bom/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}