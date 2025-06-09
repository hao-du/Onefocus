using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Abstractions.Messages;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.User;

public sealed record GetUserByIdResponseDto(Entity.User? User);
