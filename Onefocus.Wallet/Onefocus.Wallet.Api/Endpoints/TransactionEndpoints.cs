using MediatR;
using Onefocus.Wallet.Application.Transaction.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("transaction/all", (GetAllTransactionsQueryRequest query, ISender sender) =>
        {
            return true;
        });

        routes.MapGet("transaction/{id}", (GetTransactionByIdQueryRequest query, ISender sender, int id) =>
        {
            return true;
        });
    }
}
