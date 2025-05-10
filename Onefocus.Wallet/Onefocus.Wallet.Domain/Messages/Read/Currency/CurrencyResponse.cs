using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record CurrencyResponse(Guid Id, string Name, string ShortName, bool DefaultFlag, bool ActiveFlag, string? Description, DateTimeOffset? ActionedOn, Guid? ActionedBy);