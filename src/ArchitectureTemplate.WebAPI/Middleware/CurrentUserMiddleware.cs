namespace ArchitectureTemplate.WebAPI.Middleware;

public class CurrentUserMiddleware(ILogger<CurrentUserMiddleware> logger) : IMiddleware
{
    private const string CURRENT_USER_ID = "ad19affc-a438-4709-8b0b-1c2c1b2527dc"; // pretend user id    

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        UserResponse? user = null;
        try
        {
            user = GetUser();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }

        if (user == null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else
        {
            context.Items.Add("user", user);

            await next(context);
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