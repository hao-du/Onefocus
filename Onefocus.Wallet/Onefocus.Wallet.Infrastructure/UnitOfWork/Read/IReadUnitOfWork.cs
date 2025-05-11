using Onefocus.Wallet.Domain.Repositories.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

public interface IReadUnitOfWork
{
    IUserReadRepository User { get; }
    ICurrencyReadRepository Currency { get; }
}
