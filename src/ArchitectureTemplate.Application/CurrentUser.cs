﻿namespace ArchitectureTemplate.Application;

// simple implementation, not meant to be interpreted as what something like this would ultimately look like in production

public interface ICurrentUser
{
    public UserResponse? User { get; }

    bool? IsAdmin { get; }
}

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public UserResponse? User => GetAuthenticatedUserFromHttpContext();

    public bool? IsAdmin => User?.IsAdmin;

    private UserResponse? GetAuthenticatedUserFromHttpContext()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        if (httpContext.Items.TryGetValue("user", out object? user))
        {
            if (user != null && user is UserResponse userResponse)
            {
                return userResponse;
            }
        }

        return null;
    }
}

public record UserResponse(Guid UserId, bool IsAdmin);