using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Transaction.Commands.BankAccount;
public sealed record CreateBankAccountCommandRequest(
    decimal Amount,
    Guid CurrencyId,
    decimal InterestRate,
    string AccountNumber,
    DateTimeOffset IssuedOn,
    DateTimeOffset? ClosedOn,
    bool IsClosed,
    Guid BankId,
    string? Description,
    IReadOnlyList<CreateTransaction> Transactions
) : ICommand<CreateBankAccountCommandResponse>;
public sealed record CreateTransaction(
    DateTimeOffset TransactedOn,
    decimal Amount,
    string? Description
);
public sealed record CreateBankAccountCommandResponse(Guid Id);

internal sealed class CreateBankAccountCommandHandler(
    ILogger<CreateBankAccountCommandHandler> logger,
    IDomainEventService domainEventService,
    IWriteUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
    ) : CommandHandler<CreateBankAccountCommandRequest, CreateBankAccountCommandResponse>(httpContextAccessor, logger)
{
    public override async Task<Result<CreateBankAccountCommandResponse>> Handle(CreateBankAccountCommandRequest request, CancellationToken cancellationToken)
    {
        var getCurrencyResult = await unitOfWork.Currency.GetCurrencyByIdAsync(new(request.CurrencyId), cancellationToken);
        if (getCurrencyResult.IsFailure) { return Failure(getCurrencyResult); }
        var currency = getCurrencyResult.Value.Currency;

        var validationResult = ValidateRequest(request, currency);
        if (validationResult.IsFailure) return Failure(validationResult);

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return Failure(actionByResult);

        var addBankAccountResult = Entity.TransactionTypes.BankAccount.Create(
            amount: request.Amount,
            interestRate: request.InterestRate,
            currency: currency!,
            accountNumber: request.AccountNumber,
            description: request.Description,
            issuedOn: request.IssuedOn,
            closedOn: request.ClosedOn,
            isClosed: request.IsClosed,
            bankId: request.BankId,
            ownerId: actionByResult.Value,
            actionedBy: actionByResult.Value,
            transactionParams: [.. request.Transactions.Select(t => TransactionParams.CreateNew(
                amount: t.Amount,
                transactedOn: t.TransactedOn,
                currency: currency!,
                description: t.Description
            ))]
        );
        if (addBankAccountResult.IsFailure) return Failure(addBankAccountResult);

        var bankAccount = addBankAccountResult.Value;
        var repoResult = await unitOfWork.Transaction.AddBankAccountAsync(new(bankAccount), cancellationToken);
        if (repoResult.IsFailure) return Failure(repoResult);

        if (bankAccount.DomainEvents.Count > 0)
        {
            var addSearchIndexEventResult = await domainEventService.AddSearchIndexEvent(bankAccount.DomainEvents, cancellationToken);
            if (addSearchIndexEventResult.IsFailure) return Failure(addSearchIndexEventResult);
            bankAccount.ClearDomainEvents();
        }

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return Failure(saveChangesResult);

        return Result.Success<CreateBankAccountCommandResponse>(new(bankAccount.Id));
    }

    private static Result ValidateRequest(CreateBankAccountCommandRequest request, Entity.Currency? currency)
    {
        var validationResult = Entity.TransactionTypes.BankAccount.Validate(
            amount: request.Amount,
            currency: currency,
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
                currency: currency,
                transactedOn: transaction.TransactedOn
                );
            if (transactionValidationResult.IsFailure) return transactionValidationResult;
        }

        return Result.Success();
    }
}

