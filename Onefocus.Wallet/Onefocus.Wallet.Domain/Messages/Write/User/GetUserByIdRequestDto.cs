using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Write.User;

public sealed record GetUserByIdRequestDto(Guid Id);
