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
    internal class CurrencyService (IWriteUnitOfWork unitOfWork) : ICurrencyService
    {
        public async Task<Result> HasDuplicatedCurrency(Guid id, string name, string shortName, CancellationToken cancellationToken)
        {
            var orSpec = new OrSpecification<Entity.Currency>(
                FindNameSpecification<Entity.Currency>.Create(name),
                FindShortNameSpecification.Create(shortName)
            );
            var spec = orSpec.And(ExcludeIdsSpecification<Entity.Currency>.Create([id]));

            var queryResult = await unitOfWork.Currency.GetBySpecificationAsync<Entity.Currency>(new(spec), cancellationToken);
            if (queryResult.IsFailure) return queryResult;
            if (queryResult.Value.Entity is not null) return Result.Failure(Errors.Currency.NameOrShortNameIsExisted);

            return Result.Success();
        }
    }
}
