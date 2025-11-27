using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Home.Domain.Entities.ValueObjects;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onefocus.Home.Application.UseCases.Setting.Queries;

public sealed record GetAllLocaleOptionsRequest() : IQuery<GetAllLocaleOptionsResponse>;
public sealed record GetAllLocaleOptionsResponse(List<LocaleResponse> Locales);
public sealed record LocaleResponse(string Code, string NativeName);

internal sealed class GetAllLocaleOptionsQueryHandler(
    ILogger<GetAllLocaleOptionsQueryHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : QueryHandler<GetAllLocaleOptionsRequest, GetAllLocaleOptionsResponse>(httpContextAccessor, logger)
{
    private readonly string[] supportedLocales = ["vi-VN", "en-US"];

    public override Task<Result<GetAllLocaleOptionsResponse>> Handle(GetAllLocaleOptionsRequest request, CancellationToken cancellationToken)
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        return Task.Run(() => Result.Success<GetAllLocaleOptionsResponse>(new(
            [..cultures
            .Where(c => supportedLocales.Any(s => c.Name.Equals(s, StringComparison.OrdinalIgnoreCase)))
            .Select(c => new LocaleResponse(
                Code: c.Name,
                NativeName: c.NativeName
            ))]
        )));
    }
}

