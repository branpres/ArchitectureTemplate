namespace ArchitectureTemplate.WebAPI.Features;

public abstract record ResponseBase
{
    public List<Link>? Links { get; set; }
}

public record Link(string Name, string Href, string HttpMethod);