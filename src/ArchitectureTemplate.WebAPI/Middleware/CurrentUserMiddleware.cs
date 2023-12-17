namespace ArchitectureTemplate.WebAPI.Middleware;

public class CurrentUserMiddleware(RequestDelegate next, ILogger<CurrentUserMiddleware> logger)
{
    private readonly ILogger<CurrentUserMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    private const string CURRENT_USER_ID = "ad19affc-a438-4709-8b0b-1c2c1b2527dc"; // pretend user id

    public async Task InvokeAsync(HttpContext httpContext)
    {
        UserResponse? user = null;
        try
        {
            user = GetUser();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user");
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        }

        if (user == null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else
        {
            httpContext.Items.Add("user", user);

            await _next(httpContext);
        }
    }

    private static UserResponse GetUser()
    {
        /*
         * An example of retrieving the user's claims
            var user = httpContext.User;
            var claims = user.Identities.FirstOrDefault()?.Claims.ToList();
            var userName = claims?.FirstOrDefault(claim => claim.Type.Equals("cognito:username", StringComparison.OrdinalIgnoreCase))?.Value;
            if (userName == null)
            {
                return null;
            }
        */

        return new(new Guid(CURRENT_USER_ID), true);
    }
}