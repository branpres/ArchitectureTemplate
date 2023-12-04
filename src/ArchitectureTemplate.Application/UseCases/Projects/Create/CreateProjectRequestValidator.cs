namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.ProjectName).NotEmpty();
    }
}