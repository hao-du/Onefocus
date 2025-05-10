using Onefocus.Common.Abstractions.Domain.Specification;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Specifications.Currency
{
    public class FindNameSpecification : Specification<Entity.Currency>
    {
        private readonly string _name;
        private FindNameSpecification(string name)
        {
            _name = name;
        }

        public static FindNameSpecification Create(string name)
        {
            return new FindNameSpecification(name);
        }

        public override Expression<Func<Entity.Currency, bool>> ToExpression()
        {
            return c => c.Name == _name;
        }
    }
}
