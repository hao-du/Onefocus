using Microsoft.Extensions.Logging;
using Onefocus.Common.Results;

namespace Onefocus.Common.Utilities;

public static class ErrorExtensions
{
    private const string DefaultErrorTemplate = "Error with code: {Code} - description: {Description}. Extra infomation: {CustomMessage}";

    public static void LogErrors<T>(this ILogger<T> logger, IReadOnlyList<Error> errors, string? customErrorMessage = null)
    {
        foreach (var error in errors)
        {
            logger.LogError(DefaultErrorTemplate, error.Code, error.Description, customErrorMessage);
        }
    }
}