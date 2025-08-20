using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.UseCases.Transaction.Commands.CashFlow;
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
    }
}
