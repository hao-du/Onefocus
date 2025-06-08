using Onefocus.Common.Abstractions.Messages;

namespace Onefocus.Wallet.Domain.Messages.Read.User;

public sealed record GetUserByIdRequestDto(Guid Id);