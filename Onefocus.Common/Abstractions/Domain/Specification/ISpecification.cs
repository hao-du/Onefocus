using System.Linq.Expressions;

namespace Onefocus.Common.Abstractions.Domain.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
