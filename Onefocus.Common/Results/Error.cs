namespace Onefocus.Common.Results;
public record Error(string Code, string Description)
{
    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}
