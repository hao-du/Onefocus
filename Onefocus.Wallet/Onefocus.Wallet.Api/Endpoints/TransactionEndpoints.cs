using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.BankAccount;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.CashFlow;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.CurrencyExchange;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.PeerTransfer;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("transaction/all", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTransactionsQueryRequest());
            return result.ToResult();
        });

        routes.MapGet("transaction/cashflow/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCashFlowByTransactionIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("transaction/cashflow/create", async (CreateCashFlowCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPut("transaction/cashflow/update", async (UpdateCashFlowCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapGet("transaction/bankaccount/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetBankAccountByTransactionIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("transaction/bankaccount/create", async (CreateBankAccountCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPost("transaction/bankaccount/update", async (UpdateBankAccountCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapGet("transaction/currencyexchange/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCurrencyExchangeByTransactionIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("transaction/currencyexchange/create", async (CreateCurrencyExchangeCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPost("transaction/currencyexchange/update", async (UpdateCurrencyExchangeCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapGet("transaction/peertransfer/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetPeerTransferByTransactionIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("transaction/peertransfer/create", async (CreatePeerTransferCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPost("transaction/peertransfer/update", async (UpdatePeerTransferCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
