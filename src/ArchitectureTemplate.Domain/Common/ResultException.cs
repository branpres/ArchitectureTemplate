namespace ArchitectureTemplate.Domain.Common;

public class ResultException : Exception
{
    private readonly Dictionary<string, List<string>> _errors = [];

    public Dictionary<string, string[]> Errors => _errors.ToDictionary(x => x.Key, x => x.Value.ToArray());

    public ResultException() : base() { }

    public ResultException(string propertyName, string errorMessage) : base()
    {
        _errors.Add(propertyName, [errorMessage]);
    }

    public ResultException(List<ValidationFailure> validationFailures) : base()
    {
        foreach (var validationFailure in validationFailures)
        {
            if (!_errors.TryGetValue(validationFailure.PropertyName, out List<string>? errorMessages))
            {
                errorMessages = ([]);
                _errors[validationFailure.PropertyName] = errorMessages;
            }

            errorMessages.Add(validationFailure.ErrorMessage);
        }
    }

    public void AddError(string propertyName, string errorMessage)
    {
        _errors.Add(propertyName, [errorMessage]);
    }

    public void AddError(string propertyName, List<string> errorMessages)
    {
        _errors.Add(propertyName, errorMessages);
    }
}