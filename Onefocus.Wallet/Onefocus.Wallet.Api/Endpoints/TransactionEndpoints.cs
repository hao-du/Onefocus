using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Transaction.Queries;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        //routes.MapGet("transaction/all", async (GetAllTransactionsQueryRequest query, ISender sender) =>
        //{
        //    return true;
        //});

        //routes.MapGet("transaction/{id}", async (GetTransactionByIdQueryRequest query, ISender sender, int id) =>
        //{
        //    return true;
        //});
    }
}
