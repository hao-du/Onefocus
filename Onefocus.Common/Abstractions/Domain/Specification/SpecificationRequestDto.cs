namespace Onefocus.Common.Abstractions.Domain.Specification;

public record SpecificationRequestDto<TEntity>(Specification<TEntity> Specification) where TEntity : EntityBase;

