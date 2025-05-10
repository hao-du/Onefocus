using Onefocus.Common.Results;

namespace Onefocus.Common.Exceptions.Errors;

public static class CommonErrors
{
    private static readonly string _internalErrorMessage = "An internal server error has occurred.";

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Unknown = new("UnknownError", _internalErrorMessage);
    public static readonly Error InternalServer = new("InternalServerError", _internalErrorMessage);
    public static readonly Error NullReference = new("NullReference", _internalErrorMessage);
    public static readonly Error UserClaimInvalid = new("UserClaimInvalid", _internalErrorMessage);
}

