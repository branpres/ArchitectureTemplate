namespace ArchitectureTemplate.Application;

// simple implementation, not meant to be interpreted as what it would ultimately look like in production

public interface ICurrentUser
{
    public UserResponse User { get; }

    bool IsAdmin { get; }
}

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public UserResponse User => GetAuthenticatedUserFromHttpContext();

    public bool IsAdmin => User.IsAdmin;

    private UserResponse GetAuthenticatedUserFromHttpContext()
    {
        if (_httpContextAccessor.HttpContext.Items.TryGetValue("user", out object? user))
        {
            if (user != null && user is UserResponse userResponse)
            {
                return userResponse;
            }
        }

        throw new Exception("Authenticated user not found");
    }
}

public record UserResponse(Guid UserId, bool IsAdmin);