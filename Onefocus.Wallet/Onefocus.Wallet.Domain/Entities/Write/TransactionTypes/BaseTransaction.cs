using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes
{
    public abstract class BaseTransaction: WriteEntityBase
    {
        protected readonly List<Transaction> _transactions = [];
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        protected virtual Result CreateTransaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
        {
            var creationResult = Transaction.Create(amount, transactedOn, currencyId, description, actionedBy, transactionItems);
            if (creationResult.IsFailure)
            {
                return creationResult;
            }

            _transactions.Add(creationResult.Value);

            return Result.Success();
        }

        protected Result UpsertTransaction(TransactionParams? @params, Guid actionedBy)
        {
            if (@params == null)
            {
                return Result.Failure(CommonErrors.NullReference);
            }

            if (@params.Id.HasValue)
            {
                var existingTransaction = _transactions.Find(t => t.Id == @params.Id);
                if (existingTransaction == null)
                {
                    return Result.Failure(Errors.Transaction.InvalidTransaction);
                }

                var updateResult = existingTransaction.Update(@params.Amount, @params.TransactedOn, @params.CurrencyId, @params.IsActive, @params.Description, actionedBy, @params.TransactionItems);
                if (updateResult.IsFailure)
                {
                    return updateResult;
                }
            }
            else
            {
                var creationResult = CreateTransaction(@params.Amount, @params.TransactedOn, @params.CurrencyId, @params.Description, actionedBy, @params.TransactionItems);
                if (creationResult.IsFailure)
                {
                    return creationResult;
                }
            }

            return Result.Success();
        }
    }
}
