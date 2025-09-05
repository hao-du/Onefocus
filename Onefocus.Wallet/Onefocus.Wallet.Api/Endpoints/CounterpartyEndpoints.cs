using MediatR;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.UseCases.Counterparty.Commands;
using Onefocus.Wallet.Application.UseCases.Counterparty.Queries;

namespace Onefocus.Wallet.Api.Endpoints;

internal static class CounterpartyEndpoints
{
    public static void MapCounterpartyEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("counterparty/all", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllCounterpartiesQueryRequest());
            return result.ToResult();
        });

        routes.MapGet("counterparty/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCounterpartyByIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("counterparty/create", async (CreateCounterpartyCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPut("counterparty/update", async (UpdateCounterpartyCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
