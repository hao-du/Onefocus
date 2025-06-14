namespace Onefocus.Wallet.Domain.Entities.Write.Params
{
    public class CurrencyExchangeParams(decimal amount, Guid currencyId)
    {
        public decimal Amount { get; private set; } = amount;
        public Guid CurrencyId { get; private set; } = currencyId;

        public static CurrencyExchangeParams Create(decimal amount, Guid currencyId)
        {
            return new CurrencyExchangeParams(amount, currencyId);
        }
    }
}
