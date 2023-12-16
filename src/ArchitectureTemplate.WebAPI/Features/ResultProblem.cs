namespace ArchitectureTemplate.WebAPI.Features;

public class ResultProblem
{
    private readonly Dictionary<string, List<string>> _errors = [];

    public Dictionary<string, string[]> Errors => _errors.ToDictionary(x => x.Key, x => x.Value.ToArray());

    public ResultProblem() { }

    public ResultProblem(string propertyName, string errorMessage)
    {
        _errors.Add(propertyName, [errorMessage]);
    }

    public ResultProblem(List<ValidationFailure> validationFailures)
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
        if (_errors.TryGetValue(propertyName, out List<string>? errorMessages))
        {
            errorMessages.Add(errorMessage);
        }
        else
        {
            _errors[propertyName] = [errorMessage];
        }
    }
}

public class NotFoundResultProblem : ResultProblem
{
}