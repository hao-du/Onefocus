using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.UseCases.Bank.Commands;
using Onefocus.Wallet.Application.UseCases.Bank.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class BankEndpoints
{
    public static void MapBankEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapPost("bank/get", async (GetBanksQueryRequest request, ISender sender) =>
        {
            var result = await sender.Send(request);
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
