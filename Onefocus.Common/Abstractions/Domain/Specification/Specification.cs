using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Abstractions.Domain.Specification;

public abstract class Specification<T> : ISpecification<T>
{
    // Default implementation of ToExpression. This can be overridden in concrete specifications.
    public abstract Expression<Func<T, bool>> ToExpression();

    // Combine two specifications using AND logic
    public Specification<T> And(Specification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }

    // Combine two specifications using OR logic
    public Specification<T> Or(Specification<T> other)
    {
        return new OrSpecification<T>(this, other);
    }

    // Negate a specification
    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}

