namespace Onefocus.Common.Abstractions.Domain.Specification;

public record SpecificationResponseDto<TEntity>(TEntity? Entity) where TEntity : EntityBase;