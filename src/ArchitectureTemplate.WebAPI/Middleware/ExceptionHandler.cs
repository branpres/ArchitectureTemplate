namespace ArchitectureTemplate.WebAPI.Middleware;

public class ExceptionHandler(ILogger<ExceptionHandler> logger, IWebHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled Exception");

        if (!environment.IsProduction())
        {
            await httpContext.Response.WriteAsJsonAsync(exception.ToString(), cancellationToken);
        }
        else
        {
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Title = "An unrecoverable error has occurred. Please contact your administrator.",
                Status = HttpStatusCode.InternalServerError,
                TraceId = Activity.Current?.TraceId.ToString() ?? httpContext?.TraceIdentifier,
            }, cancellationToken);
        }

        return true;
    }
}