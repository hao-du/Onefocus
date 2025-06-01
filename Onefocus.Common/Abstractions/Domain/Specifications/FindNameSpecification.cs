using Onefocus.Common.Abstractions.Domain.Fields;
using Onefocus.Common.Abstractions.Domain.Specification;
using System.Linq.Expressions;

namespace Onefocus.Common.Abstractions.Domain.Specifications
{
    public class FindNameSpecification<T> : Specification<T> where T : INameField
    {
        private readonly string _name;

        private FindNameSpecification(string name)
        {
            _name = name;
        }

        public static FindNameSpecification<T> Create(string name)
        {
            return new FindNameSpecification<T>(name);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return c => c.Name == _name;
        }
    }
}
