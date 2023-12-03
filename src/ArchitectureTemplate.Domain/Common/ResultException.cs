namespace ArchitectureTemplate.Domain.Common;

public class ResultException : Exception
{
    public Dictionary<string, string[]> Errors { get; } = [];

    public ResultException(string propertyName, string errorMessage) : base()
    {
        Errors.Add(propertyName, [errorMessage]);
    }

    public ResultException(Dictionary<string, string[]> errors) : base()
    {
        Errors = errors;
    }

    public void AddError(string propertyName, string errorMessage)
    {
        Errors.Add(propertyName, [errorMessage]);
    }

    public void AddError(string propertyName, string[] errorMessages)
    {
        Errors.Add(propertyName, errorMessages);
    }
}