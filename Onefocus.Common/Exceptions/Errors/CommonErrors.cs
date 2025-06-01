using Onefocus.Common.Results;

namespace Onefocus.Common.Exceptions.Errors;

public static class CommonErrors
{
    public const string InternalErrorMessage = "An internal server error has occurred.";

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Unknown = new("UnknownError", InternalErrorMessage);
    public static readonly Error InternalServer = new("InternalServerError", InternalErrorMessage);
    public static readonly Error NullReference = new("NullReference", InternalErrorMessage);
    public static readonly Error UserClaimInvalid = new("UserClaimInvalid", InternalErrorMessage);
}

