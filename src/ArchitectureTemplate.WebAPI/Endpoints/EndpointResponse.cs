namespace ArchitectureTemplate.WebAPI.Endpoints;

public record EndpointResponse<T>(T Response, List<Link> Links);

public record Link(string Name, string Href, string HttpMethod);

public static class EndpointResponseMapper
{
    public static EndpointResponse<T> Map<T>(this T Response, List<Link> links)
    {
        return new EndpointResponse<T>(Response, links);
    }
}