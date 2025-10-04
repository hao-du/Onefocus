using MediatR;
using Onefocus.Common.Results;
using Onefocus.Home.Application.UseCases.Setting.Queries;
using Onefocus.Wallet.Application.UseCases.Bank.Commands;
using static Onefocus.Common.Results.ResultExtensions;

namespace Onefocus.Home.Api.Endpoints;

internal static class SettingEndpoints
{
    public static void MapSettingEndpoints(this IEndpointRouteBuilder app)
    {
        var routes = app.MapGroup(prefix: string.Empty).RequireAuthorization();

        routes.MapGet("setting/get", async (ISender sender) =>
        {
            var result = await sender.Send(new GetSettingByUserIdQueryRequest());
            return result.ToResult();
        });

        routes.MapGet("setting/option/locales", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllLocaleOptionsRequest());
            return result.ToResult();
        });

        routes.MapGet("setting/option/timezones", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTimeZonesRequest());
            return result.ToResult();
        });

        routes.MapPost("setting/upsert", async (UpsertSettingCommandRequest command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
