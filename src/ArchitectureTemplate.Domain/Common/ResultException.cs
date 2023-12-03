namespace ArchitectureTemplate.Domain.Common;

public class ResultException : Exception
{
    public Dictionary<string, string[]>? Errors { get; }

    public ResultException(string message) : base(message) { }

    public ResultException(Dictionary<string, string[]> errors) : base()
    {
        Errors = errors;
    }
}