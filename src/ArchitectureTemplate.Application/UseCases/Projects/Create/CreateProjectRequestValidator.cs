namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.ProjectIdentifier)
            .MaximumLength(64);
    }
}