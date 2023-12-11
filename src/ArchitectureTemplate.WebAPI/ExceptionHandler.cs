namespace ArchitectureTemplate.WebAPI;

internal class ExceptionHandler(ILogger<ExceptionHandler> logger, IWebHostEnvironment environment) : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled Exception");

        if (!_environment.IsProduction())
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