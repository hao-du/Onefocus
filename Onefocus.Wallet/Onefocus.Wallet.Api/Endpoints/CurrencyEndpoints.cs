using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.User.Commands;
using Onefocus.Wallet.Application.Transaction.Queries;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            Result result = await sender.Send(new GetCurrencyByIdQueryRequest(id));
            return result.ToResult();
        });

        routes.MapPost("currency/create", async (CreateCurrencyCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);
            return result.ToResult();
        });

        routes.MapPut("currency/update", async (CreateCurrencyCommandRequest command, ISender sender) =>
        {
            Result result = await sender.Send(command);
            return result.ToResult();
        });
    }
}
