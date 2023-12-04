namespace ArchitectureTemplate.Application;

// simple implementation, not meant to be interpreted as what it would ultimately look like in production

public interface ICurrentUser
{
    Guid UserId { get; }

    bool IsAdmin { get; }
}

public class CurrentUser : ICurrentUser
{
    public const string CURRENT_USER_ID = "ad19affc-a438-4709-8b0b-1c2c1b2527dc";

    public Guid UserId => new(CURRENT_USER_ID);

    public bool IsAdmin => true;
}