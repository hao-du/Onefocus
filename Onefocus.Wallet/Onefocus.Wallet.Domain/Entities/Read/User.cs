using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Read.Transactions;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class User : ReadEntityBase
{
    private List<Transaction> _transactions = new List<Transaction>();
    private List<TransferTransaction> _transferTransactions = new List<TransferTransaction>();

    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<TransferTransaction> TransferTransactions => _transferTransactions.AsReadOnly();
}