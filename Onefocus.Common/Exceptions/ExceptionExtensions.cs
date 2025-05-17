using Microsoft.AspNetCore.Http;
using static Onefocus.Common.Results.ExceptionExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace Onefocus.Common.Results;

public static class ExceptionExtensions
{
    public static List<Error> ToErrors(this Exception ex)
    {
        var errors = new List<Error>();

        ConvertToErrors(ex, errors);

        return errors;
    }

    public static List<Error> ToError(this Exception ex)
    {
        var errors = new List<Error>();

        ConvertToErrors(ex, errors, false);

        return errors;
    }

    private static void ConvertToErrors(Exception ex, List<Error> errors, bool getRecursive = true)
    {
        if (errors == null) return;

        errors.Add(new Error(ex.Source ?? string.Empty, ex.Message));

        if (ex.InnerException != null && getRecursive)
        {
            ConvertToErrors(ex.InnerException, errors);
        }
    }
}
