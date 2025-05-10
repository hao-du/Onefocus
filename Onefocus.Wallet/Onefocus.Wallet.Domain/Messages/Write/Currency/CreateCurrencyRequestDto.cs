using MediatR;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Messages.Write;

public sealed record CreateCurrencyRequestDto(string Name, string ShortName, bool DefaultFlag, string? Description, Guid CreatedBy)
{
    public Result<Currency> ToObject()
    {
        return Currency.Create(Name, ShortName, Description, DefaultFlag, CreatedBy);
    }
}
