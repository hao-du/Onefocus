using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.User;

public sealed record GetUserByIdResponseDto(Entity.User? User);
