namespace ArchitectureTemplate.Tests.Integration;

[Collection("TestCollection")]
internal abstract class IntegrationTestBase(IntegrationTestWebApplicationFactory webApplicationFactory)
{
    protected HttpClient? _httpClient = webApplicationFactory.HttpClient;
}