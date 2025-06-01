using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.Domain.Specification;
using System.Linq.Expressions;

namespace Onefocus.Wallet.Domain.Specifications
{
    public class ExcludeIdsSpecification<T> : Specification<T> where T : EntityBase
    {
        private readonly List<Guid> _ids;

        private ExcludeIdsSpecification(List<Guid> ids)
        {
            _ids = ids;
        }

        public static ExcludeIdsSpecification<T> Create(List<Guid> ids)
        {
            return new ExcludeIdsSpecification<T>(ids);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return c => !_ids.Contains(c.Id);
        }
    }
}
