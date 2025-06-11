using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.UseCases.Transaction.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("trace/all", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTransactionsQueryRequest());
            return result.ToResult();
        });
    }
}
