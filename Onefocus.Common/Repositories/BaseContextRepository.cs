using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Results;

namespace Onefocus.Common.Repositories;

public abstract class BaseContextRepository<TRepository>(ILogger<TRepository> logger, DbContext context) : BaseRepository<TRepository>(logger), IBaseContextRepository
{
    private DbContext DbContext { get; } = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Result<SpecificationListResponseDto<TEntity>>> GetListBySpecificationAsync<TEntity>(SpecificationRequestDto<TEntity> request, CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        return await ExecuteAsync(async () =>
        {
            var entities = await DbContext.Set<TEntity>()
                .Where(request.Specification.ToExpression())
                .ToListAsync(cancellationToken);

            return Result.Success<SpecificationListResponseDto<TEntity>>(new(entities));
        });
    }

    public async Task<Result<SpecificationResponseDto<TEntity>>> GetBySpecificationAsync<TEntity>(SpecificationRequestDto<TEntity> request, CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        return await ExecuteAsync(async () =>
        {
            var entity = await DbContext.Set<TEntity>()
                .Where(request.Specification.ToExpression())
                .FirstOrDefaultAsync(cancellationToken);

            return Result.Success<SpecificationResponseDto<TEntity>>(new(entity));
        });
    }
}
