using System.Linq.Expressions;

namespace Onefocus.Common.Abstractions.Domain.Specification;

public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = left.ToExpression();
        var rightExpression = right.ToExpression();
        var parameter = Expression.Parameter(typeof(T), "x");

        var andExpression = Expression.AndAlso(
            Expression.Invoke(leftExpression, parameter),
            Expression.Invoke(rightExpression, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
    }
}
