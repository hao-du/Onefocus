using Onefocus.Common.Exceptions.Errors;
using System.Diagnostics.CodeAnalysis;

namespace Onefocus.Common.Results;

public record Error(string Code, string Description)
{
    public static implicit operator Result(Error error) => Result.Failure(error);
    public Result ToResult() => Result.Failure(this);
}

public class Result
{
    protected internal Result(bool isSuccess, List<Error> errors)
    {
        if (isSuccess && errors.Count > 0)
        {
            throw new ArgumentException("Invalid errors.", nameof(errors));
        }

        IsSuccess = isSuccess;
        _errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private readonly List<Error> _errors;
    public IReadOnlyList<Error> Errors => [.. _errors];
    public Error Error => _errors.First();

    public static Result Success() => new(true, []);

    public static Result<TValue> Success<TValue>(TValue? value) => new(value, true, []);

    public static Result Failure(Error error) => new(false, [error]);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, [error]);

    public static Result Failure(List<Error> errors) => new(false, errors);

    public static Result<TValue> Failure<TValue>(List<Error> errors) => new(default, false, errors);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, List<Error> errors) : base(isSuccess, errors)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(CommonErrors.NullReference);
}

