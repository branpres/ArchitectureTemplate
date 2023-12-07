namespace ArchitectureTemplate.Tests.Integration.Projects;

public class CreateTests(IntegrationTestWebApplicationFactory webApplicationFactory) : IntegrationTestBase(webApplicationFactory)
{
    [Fact]
    public async Task ShouldCreateProject()
    {
        var response = await CreateProject(new CreateProjectRequest(Guid.NewGuid(), "Test"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ShouldCreateProjectAndGetItBack()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var response = await CreateProjectAndGetItBack(createProjectRequest);
        var endpointResponse = await response.Content.ReadFromJsonAsync<EndpointResponse<GetProjectByIdResponse>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(endpointResponse!.Response.CompanyId == createProjectRequest.CompanyId
            && endpointResponse.Response.ProjectName == createProjectRequest.ProjectName
            && endpointResponse.Response.ProjectIdentifier == createProjectRequest.ProjectIdentifier);
    }

    [Fact]
    public async Task ShouldCreateProjectWithNoAdminUser()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var response = await CreateProjectAndGetItBack(createProjectRequest);
        var endpointResponse = await response.Content.ReadFromJsonAsync<EndpointResponse<GetProjectByIdResponse>>();

        Assert.False(endpointResponse!.Response.ProjectUsers!.Single().IsAdmin);
    }

    [Fact]
    public async Task ShouldCreateProjectWithAdminUser()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid(), Guid.NewGuid());
        var response = await CreateProjectAndGetItBack(createProjectRequest);
        var endpointResponse = await response.Content.ReadFromJsonAsync<EndpointResponse<GetProjectByIdResponse>>();

        Assert.Equal(2, endpointResponse!.Response.ProjectUsers!.Count);
        Assert.Single(endpointResponse.Response.ProjectUsers.Where(x => x.IsAdmin));
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "Test", "Test")]
    [InlineData("b0bd7b02-0947-4438-bb1d-a5e19cf8e6be", "", "Test")]
    [InlineData("b0bd7b02-0947-4438-bb1d-a5e19cf8e6be", "11111111111111111111111111111111111111111111111111111111111111111", "Test")]
    [InlineData("b0bd7b02-0947-4438-bb1d-a5e19cf8e6be", "Test", "11111111111111111111111111111111111111111111111111111111111111111")]
    public async Task ShouldNotCreateProjectWithFluentValidationFailures(string companyId, string projectName, string projectIdentifier)
    {
        var response = await CreateProject(new CreateProjectRequest(new Guid(companyId), projectName, projectIdentifier));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ShouldAllowIdenticallyNamedProjectsToBeCreatedFromDifferentCompanies()
    {
        await CreateProject(new CreateProjectRequest(Guid.NewGuid(), "Test", "Test"));
        var response = await CreateProject(new CreateProjectRequest(Guid.NewGuid(), "Test", "Test"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotCreateProjectIfProjectNameExistsForTheCompany()
    {
        var companyId = Guid.NewGuid();

        await CreateProject(new CreateProjectRequest(companyId, "Test", "Test"));

        // try to create another project with the same name, but different identifier
        var response = await CreateProject(new CreateProjectRequest(companyId, "Test", "Test 2"));
        var problemDetails = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.Equal("Project name already exists for this company.", problemDetails!.Errors.Single().Value.Single());
    }

    [Fact]
    public async Task ShouldNotCreateProjectIfProjectIdentifierExistsForTheCompany()
    {
        var companyId = Guid.NewGuid();

        await CreateProject(new CreateProjectRequest(companyId, "Test", "Test"));

        // try to create another project with the same identifier, but different name
        var response = await CreateProject(new CreateProjectRequest(companyId, "Test 2", "Test"));
        var problemDetails = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.Equal("Project identifier already exists for this company.", problemDetails!.Errors.Single().Value.Single());
    }

    [Fact]
    public async Task ShouldCreateBillOfMaterialsAfterCreatingProject()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var response = await CreateProjectAndGetItBack(createProjectRequest);
        var endpointResponse = await response.Content.ReadFromJsonAsync<EndpointResponse<GetProjectByIdResponse>>();

        var getBillOfMaterialsLink = endpointResponse!.Links!.First(x => x.Name == "GetBillOfMaterialsByProjectId").Href;
        var billOfMaterialsResponse = await _httpClient.GetAsync(getBillOfMaterialsLink);
        var endpointResponseForBom = await billOfMaterialsResponse.Content.ReadFromJsonAsync<EndpointResponse<GetBillOfMaterialsByProjectIdResponse>>();

        Assert.True(endpointResponseForBom!.Response.ProjectId == endpointResponse!.Response.ProjectId
            && endpointResponseForBom.Response.BillOfMaterialsName == endpointResponse.Response.ProjectName);
    }

    [Fact]
    public async Task ShouldCreateScopePackageAfterCreatingProject()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var response = await CreateProjectAndGetItBack(createProjectRequest);
        var endpointResponse = await response.Content.ReadFromJsonAsync<EndpointResponse<GetProjectByIdResponse>>();

        var getScopePackageLink = endpointResponse!.Links!.First(x => x.Name == "GetScopeByProjectId").Href;
        var scopePackageResponse = await _httpClient.GetAsync(getScopePackageLink);
        var endpointResponseForScopePackage = await scopePackageResponse.Content.ReadFromJsonAsync<EndpointResponse<List<GetScopePackagesByProjectIdResponse>>>();

        Assert.True(endpointResponseForScopePackage!.Response.Single().ProjectId == endpointResponse!.Response.ProjectId
            && endpointResponseForScopePackage.Response.Single().ScopePackageName == ScopePackage.DEFAULT_SCOPE_PACKAGE_NAME);
    }
}