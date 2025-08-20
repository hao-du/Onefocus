using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Specifications.Write.Currency;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Services
{
    internal class BankService(IWriteUnitOfWork unitOfWork) : IBankService
    {
        public async Task<Result> HasDuplicatedBank(Guid id, string name, CancellationToken cancellationToken)
        {
            var spec = FindNameSpecification<Entity.Bank>.Create(name).And(ExcludeIdsSpecification<Entity.Bank>.Create([id]));
            var queryResult = await unitOfWork.Bank.GetBySpecificationAsync<Entity.Bank>(new(spec), cancellationToken);
            if (queryResult.IsFailure) return queryResult;
            if (queryResult.Value.Entity is not null) return Result.Failure(Errors.Bank.NameIsExisted);

            return Result.Success();
        }
    }
}
