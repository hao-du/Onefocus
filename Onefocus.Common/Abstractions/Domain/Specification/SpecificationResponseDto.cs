namespace Onefocus.Common.Abstractions.Domain.Specification;

public record SpecificationResponseDto<TEntity>(TEntity? Value) where TEntity : EntityBase;