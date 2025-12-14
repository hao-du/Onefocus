using Onefocus.Common.Results;

namespace Onefocus.Common.Utilities;

public static class HttpExtensions
{
    public static Error GetError(this HttpResponseMessage httpResponse)
    {
        return new Error(httpResponse.StatusCode.ToString(), httpResponse.ReasonPhrase ?? string.Empty);
    }
}
