using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.User;

public sealed record AddUserRequestDto(Entity.User User);
