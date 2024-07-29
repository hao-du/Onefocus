using Microsoft.AspNetCore.Http;
using static Onefocus.Common.Results.ResultExtensions;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace Onefocus.Common.Results;

public static class ResultExtensions
{
    public sealed record HttpOkResults(int Status, string Title, string Type);
    public sealed record HttpOkResults<TResponse>(int Status, string Title, string Type, TResponse Value);

    public static IResult ToBadRequestProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't convert success result to problem.");
        }

        return HttpResults.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error } }
            });
    }
    
    public static IResult ToOk(this Result result) 
    {
        if (result.IsFailure)
        {
            throw new InvalidOperationException("Can't convert failed result to success.");
        }

        var okResult = new HttpOkResults(StatusCodes.Status200OK, "OK", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1");

        return HttpResults.Ok(okResult);
    }
    
    public static IResult ToOk<TResponse>(this Result<TResponse> result)
    {
        if (result.IsFailure)
        {
            throw new InvalidOperationException("Can't convert failed result to success.");
        }

        var okResult = new HttpOkResults<TResponse>(StatusCodes.Status200OK, "OK", "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1", result.Value);

        return HttpResults.Ok(okResult);
    }

}
