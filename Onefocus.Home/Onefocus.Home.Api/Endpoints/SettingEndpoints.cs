using MediatR;
using Onefocus.Common.Results;
using Onefocus.Home.Application.UseCases.Settings.Queries;
using Onefocus.Wallet.Application.UseCases.Bank.Commands;
using static Onefocus.Common.Results.ResultExtensions;

namespace Onefocus.Home.Api.Endpoints;

internal static class SettingEndpoints
{
    public static void MapSettingEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("settings/get", async (ISender sender) =>
        {
            var result = await sender.Send(new GetSettingsByUserIdQueryRequest());
            return result.ToResult();
        });

        routes.MapGet("settings/option/locales", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllLocaleOptionsRequest());
            return result.ToResult();
        });

        routes.MapGet("settings/option/timezones", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTimeZonesRequest());
            return result.ToResult();
        });

        routes.MapPost("settings/upsert", async (UpsertSettingCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
