using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.UseCases.Currency.Commands;
using Onefocus.Wallet.Application.UseCases.Currency.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class CurrencyEndpoints
{
    public static void MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("currency/all", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllCurrenciesQueryRequest());
            return result.ToResult();
        });

        routes.MapGet("currency/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCurrencyByIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("currency/create", async (CreateCurrencyCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPut("currency/update", async (UpdateCurrencyCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
