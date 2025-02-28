using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class RequestValidationException : Exception
{
    public List<ValidationFailure>? Errors { get; init; }

    public RequestValidationException(List<ValidationFailure>? errors) : base("User input validation failed!")
    {
        Errors = errors;
    }

    public RequestValidationException(string? message) : base(message)
    {
    }
}
