using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Bank.Commands;
using Onefocus.Wallet.Application.Bank.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class BankEndpoints
{
    public static void MapBankEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("bank/all", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllBanksQueryRequest());
            return result.ToResult();
        });

        routes.MapGet("bank/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetBankByIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("bank/create", async (CreateBankCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPut("bank/update", async (UpdateBankCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
