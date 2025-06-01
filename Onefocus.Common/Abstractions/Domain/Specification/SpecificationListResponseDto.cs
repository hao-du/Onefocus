namespace Onefocus.Common.Abstractions.Domain.Specification;

public record SpecificationListResponseDto<TEntity>(List<TEntity> Values) where TEntity : EntityBase;