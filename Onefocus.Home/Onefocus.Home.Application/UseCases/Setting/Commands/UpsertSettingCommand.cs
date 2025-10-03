using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Home.Domain;
using Onefocus.Home.Domain.Entities.ValueObjects;
using Onefocus.Home.Domain.Entities.Write.Params;
using Entity = Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.UseCases.Bank.Commands;

public sealed record UpsertSettingCommandRequest(string Locale, string Timezone) : ICommand;

internal sealed class UpsertSettingCommandHandler(
    ILogger<UpsertSettingCommandHandler> logger,
    IWriteUnitOfWork writeUnitOfWork,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<UpsertSettingCommandRequest>(httpContextAccessor, logger)
{
    public override async Task<Result> Handle(UpsertSettingCommandRequest request, CancellationToken cancellationToken)
    {
        var preferenceParams = PreferenceParams.Create(request.Locale, request.Timezone);

        var validationResult = ValidateRequest(preferenceParams);
        if (validationResult.IsFailure) return validationResult;

        var actionByResult = GetUserId();
        if (actionByResult.IsFailure) return actionByResult;
        var userId = actionByResult.Value;

        var getSettingResult = await writeUnitOfWork.Setting.GetSettingByUserIdAsync(new(userId));
        if (getSettingResult.IsFailure) return getSettingResult;
        var setting = getSettingResult.Value.Setting;

        if (setting == null)
        {
            var createSettingResult = Entity.Setting.Create(preferenceParams, userId);
            if(createSettingResult.IsFailure) return createSettingResult;

            var addSettingToContextResult = await writeUnitOfWork.Setting.AddSettingAsync(new(createSettingResult.Value), cancellationToken);
            if (addSettingToContextResult.IsFailure) return addSettingToContextResult;
        }
        else
        {
            var updateSettingResult = setting.Update(preferenceParams, userId);
            if (updateSettingResult.IsFailure) return updateSettingResult;
        }

        var saveChangesResult = await writeUnitOfWork.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsFailure) return saveChangesResult;

        return Result.Success();
    }

    private Result ValidateRequest(PreferenceParams preferenceParams)
    {
        var validationResult = Preferences.Validate(preferenceParams);
        return validationResult;
    }
}
