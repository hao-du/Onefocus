using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.User;

public sealed record GetUserByIdResponseDto(Entity.User? User);
