﻿namespace ArchitectureTemplate.WebAPI.Features.Projects;

public class DeleteProjectEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/project/{projectId}", Delete)
            .WithOpenApi(x => new(x)
            {
                OperationId = "DeleteProject",
                Tags = new List<OpenApiTag> { new() { Name = "Projects" } },
                Description = "Deletes a project."
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return builder;
    }

    private static async Task<IResult> Delete(
        Guid projectId,
        TemplateDbContext templateDbContext,
        CancellationToken cancellationToken)
    {
        var handler = new DeleteProjectHandler(templateDbContext);
        var result = await handler.Handle(projectId, cancellationToken);

        return result.Match(
            Results.NoContent,
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }
}

public class DeleteProjectHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid>
{
    public async Task<Result> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await templateDbContext.Project.GetProjectWithProjectUsers(projectId, cancellationToken);

        if (project == null)
        {
            return new Result(new NotFoundResultProblem());
        }

        project.SoftDelete();
        await templateDbContext.SaveChangesAsync(cancellationToken);

        return new Result();
    }
}