namespace Onefocus.Common.Abstractions.Domain.Specification;

public record SpecificationListResponseDto<TEntity>(List<TEntity> Entities) where TEntity : EntityBase;