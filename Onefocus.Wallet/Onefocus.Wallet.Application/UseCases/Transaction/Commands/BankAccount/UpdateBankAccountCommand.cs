using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.BankAccount;
public sealed record UpdateBankAccountCommandRequest(
    Guid Id,
    decimal Amount,
    Guid CurrencyId,
    decimal InterestRate,
    string AccountNumber,
    DateTimeOffset IssuedOn,
    DateTimeOffset? ClosedOn,
    bool IsClosed,
    Guid BankId,
    string? Description,
    bool IsActive,
    IReadOnlyList<UpdateTransaction> Transactions
) : ICommand;
public sealed record UpdateTransaction(
    Guid? Id,
    DateTimeOffset TransactedOn,
    decimal Amount,
    string? Description,
    bool IsActive
);

internal sealed class UpdateBankAccountCommandHandler(
        IWriteUnitOfWork unitOfWork
        , IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<UpdateBankAccountCommandRequest>(httpContextAccessor)
{
    public override async Task<Result> Handle(UpdateBankAccountCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;

        var getBankAccountResult = await unitOfWork.Transaction.GetBankAccountByIdAsync(new(request.Id), cancellationToken);
        if (getBankAccountResult.IsFailure) return getBankAccountResult;

        var bankAccount = getBankAccountResult.Value.BankAccount;
        if (bankAccount == null) return Result.Failure(CommonErrors.NullReference);

        var updateBankAccountResult = bankAccount.Update(
           amount: request.Amount,
            interestRate: request.InterestRate,
            currencyId: request.CurrencyId,
            accountNumber: request.AccountNumber,
            description: request.Description,
            issuedOn: request.IssuedOn,
            closedOn: request.ClosedOn,
            isClosed: request.IsClosed,
            bankId: request.BankId,
            actionedBy: actionByResult.Value,
            isActive: request.IsActive,
            transactionParams: [.. request.Transactions.Select(t => TransactionParams.Create(
                id: t.Id,
                amount: t.Amount,
                transactedOn: t.TransactedOn,
                currencyId: request.CurrencyId,
                description: t.Description,
                isActive: t.IsActive
            ))]
        );
        if (updateBankAccountResult.IsFailure) return updateBankAccountResult;

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private static Result ValidateRequest(UpdateBankAccountCommandRequest request)
    {
        var validationResult = Entity.TransactionTypes.BankAccount.Validate(
            amount: request.Amount,
            currencyId: request.CurrencyId,
            bankId: request.BankId,
            interestRate: request.InterestRate,
            issuedOn: request.IssuedOn,
            isClosed: request.IsClosed,
            accountNumber: request.AccountNumber,
            closedOn: request.ClosedOn);
        if (validationResult.IsFailure) return validationResult;

        foreach (var transaction in request.Transactions)
        {
            var transactionValidationResult = Entity.Transaction.Validate(
                amount: transaction.Amount,
                currencyId: request.CurrencyId,
                transactedOn: transaction.TransactedOn
                );
            if (transactionValidationResult.IsFailure) return transactionValidationResult;
        }

        return Result.Success();
    }
}

