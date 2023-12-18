namespace ArchitectureTemplate.Tests.Validation.Projects;

public class CreateTests
{
    [Fact]
    public void ShouldValidateRequest()
    {
        var validator = new CreateProjectRequestValidator();

        var validationResult = validator.TestValidate(new CreateProjectRequest(Guid.NewGuid(), "Test", "Test"));

        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "Test", "Test", "CompanyId", "NotEmptyValidator")]
    [InlineData("b0bd7b02-0947-4438-bb1d-a5e19cf8e6be", "", "Test", "ProjectName", "NotEmptyValidator")]
    [InlineData("b0bd7b02-0947-4438-bb1d-a5e19cf8e6be", "11111111111111111111111111111111111111111111111111111111111111111", "Test", "ProjectName", "MaximumLengthValidator")]
    [InlineData("b0bd7b02-0947-4438-bb1d-a5e19cf8e6be", "Test", "11111111111111111111111111111111111111111111111111111111111111111", "ProjectIdentifier", "MaximumLengthValidator")]
    public void ShouldNotValidateRequest(string companyId, string projectName, string projectIdentifier, string propertyNameWithExpectedErrorCode, string expectedErrorCode)
    {
        var validator = new CreateProjectRequestValidator();

        var validationResult = validator.TestValidate(new CreateProjectRequest(new Guid(companyId), projectName, projectIdentifier));

        validationResult.ShouldHaveValidationErrorFor(propertyNameWithExpectedErrorCode).WithErrorCode(expectedErrorCode);
    }
}