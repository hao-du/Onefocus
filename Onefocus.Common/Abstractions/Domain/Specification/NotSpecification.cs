using System.Linq.Expressions;

namespace Onefocus.Common.Abstractions.Domain.Specification;

public class NotSpecification<T>(Specification<T> specification) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = specification.ToExpression();
        var parameter = Expression.Parameter(typeof(T), "x");

        var notExpression = Expression.Not(Expression.Invoke(expression, parameter));

        return Expression.Lambda<Func<T, bool>>(notExpression, parameter);
    }
}
