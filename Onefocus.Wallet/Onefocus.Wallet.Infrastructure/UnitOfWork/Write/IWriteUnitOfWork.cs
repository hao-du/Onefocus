using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Repositories.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

public interface IWriteUnitOfWork
{
    IUserWriteRepository User { get; }
    IBankWriteRepository Bank { get; }
    ICurrencyWriteRepository Currency { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default);
}