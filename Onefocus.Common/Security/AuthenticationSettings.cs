using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Membership.Api.Security
{
    public interface IAuthenticationSettings
    {
        string TokenProviderName { get; }
        string Audience { get; }
        string Issuer { get; }
        string SymmetricSecurityKeyString { get; }
        long AuthTokenExpirySpanSeconds { get; }
    }

    public class AuthenticationSettings : IAuthenticationSettings
    {
        public string TokenProviderName { get; init; } = string.Empty;
        public string SymmetricSecurityKeyString { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public long AuthTokenExpirySpanSeconds { get; init; }
    }
}
