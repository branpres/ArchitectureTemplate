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
        var httpResponse = await CreateProjectAndGetItBack(createProjectRequest);
        var response = await httpResponse.Content.ReadFromJsonAsync<GetProjectByIdResponse>();

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.True(response!.CompanyId == createProjectRequest.CompanyId
            && response.ProjectName == createProjectRequest.ProjectName
            && response.ProjectIdentifier == createProjectRequest.ProjectIdentifier);
    }

    [Fact]
    public async Task ShouldCreateProjectWithNoAdminUser()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var httpResponse = await CreateProjectAndGetItBack(createProjectRequest);
        var response = await httpResponse.Content.ReadFromJsonAsync<GetProjectByIdResponse>();

        Assert.False(response!.ProjectUsers!.Single().IsAdmin);
    }

    [Fact]
    public async Task ShouldCreateProjectWithAdminUser()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid(), Guid.NewGuid());
        var httpResponse = await CreateProjectAndGetItBack(createProjectRequest);
        var response = await httpResponse.Content.ReadFromJsonAsync<GetProjectByIdResponse>();

        Assert.Equal(2, response!.ProjectUsers!.Count);
        Assert.Single(response.ProjectUsers.Where(x => x.IsAdmin));
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
        var createProjectResponse = await CreateProjectAndGetItBack(createProjectRequest);
        var response = await createProjectResponse.Content.ReadFromJsonAsync<GetProjectByIdResponse>();

        var getBillOfMaterialsLink = response!.Links!.First(x => x.Name == "GetBillOfMaterialsByProjectId").Href;
        var billOfMaterialsResponse = await _httpClient.GetAsync(getBillOfMaterialsLink);
        var responseForBom = await billOfMaterialsResponse.Content.ReadFromJsonAsync<GetBillOfMaterialsByProjectIdResponse>();

        Assert.True(responseForBom!.ProjectId == response!.ProjectId
            && responseForBom.BillOfMaterialsName == response.ProjectName);
    }

    [Fact]
    public async Task ShouldCreateScopePackageAfterCreatingProject()
    {
        var createProjectRequest = new CreateProjectRequest(Guid.NewGuid(), "Test", "Test", Guid.NewGuid());
        var createProjectResponse = await CreateProjectAndGetItBack(createProjectRequest);
        var response = await createProjectResponse.Content.ReadFromJsonAsync<GetProjectByIdResponse>();

        var getScopePackageLink = response!.Links!.First(x => x.Name == "GetScopePackagesByProjectId").Href;
        var scopePackageResponse = await _httpClient.GetAsync(getScopePackageLink);
        var responseForScopePackage = await scopePackageResponse.Content.ReadFromJsonAsync<List<GetScopePackagesByProjectIdResponse>>();

        Assert.True(responseForScopePackage!.Single().ProjectId == response!.ProjectId
            && responseForScopePackage!.Single().ScopePackageName == ScopePackage.DEFAULT_SCOPE_PACKAGE_NAME);
    }
}