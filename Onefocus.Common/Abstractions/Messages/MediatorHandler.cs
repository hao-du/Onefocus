using Microsoft.AspNetCore.Http;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using System.Security.Claims;

namespace Onefocus.Common.Abstractions.Messages;

public abstract class MediatorHandler(IHttpContextAccessor httpContextAccessor)
{
    protected Result<Guid> GetUserId()
    {
        if (!Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            return Result.Failure<Guid>(CommonErrors.UserClaimInvalid);
        }

        return userId;
    }

    protected string? GetCookie(string key)
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[key];
    }

    protected void AppendCookie(string key, string value, SameSiteMode mode = SameSiteMode.Unspecified, bool httpOnly = true)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, new CookieOptions
        {
            SameSite = mode,
            HttpOnly = httpOnly
        });
    }
}