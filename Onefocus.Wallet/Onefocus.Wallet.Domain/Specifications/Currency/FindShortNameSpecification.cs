using Onefocus.Common.Abstractions.Domain.Specification;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Specifications.Currency
{
    public class FindShortNameSpecification : Specification<Entity.Currency>
    {
        private readonly string _shortName;
        private FindShortNameSpecification(string shortName)
        {
            _shortName = shortName;
        }

        public static FindShortNameSpecification Create(string shortName)
        {
            return new FindShortNameSpecification(shortName);
        }

        public override Expression<Func<Entity.Currency, bool>> ToExpression()
        {
            return c => c.ShortName == _shortName;
        }
    }
}
