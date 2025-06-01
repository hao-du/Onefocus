using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Results;

namespace Onefocus.Common.Repositories;

public interface IBaseContextRepository : IBaseRepository
{
    Task<Result<SpecificationListResponseDto<TEntity>>> GetListBySpecificationAsync<TEntity>(SpecificationRequestDto<TEntity> request, CancellationToken cancellationToken = default)
        where TEntity : EntityBase;
    Task<Result<SpecificationResponseDto<TEntity>>> GetBySpecificationAsync<TEntity>(SpecificationRequestDto<TEntity> request, CancellationToken cancellationToken = default)
        where TEntity : EntityBase;
}
