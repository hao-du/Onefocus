using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Read;
using static Onefocus.Wallet.Domain.Messages.Read.User.GetAllUsersResponseDto;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetCurrencyByIdRequestDto(Guid Id);