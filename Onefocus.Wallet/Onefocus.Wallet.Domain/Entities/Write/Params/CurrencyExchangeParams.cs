namespace Onefocus.Wallet.Domain.Entities.Write.Params
{
    public class CurrencyExchangeParams(decimal amount, Currency? currency)
    {
        public decimal Amount { get; private set; } = amount;
        public Currency? Currency { get; private set; } = currency;

        public static CurrencyExchangeParams Create(decimal amount, Currency? currency)
        {
            return new CurrencyExchangeParams(amount, currency);
        }
    }
}
