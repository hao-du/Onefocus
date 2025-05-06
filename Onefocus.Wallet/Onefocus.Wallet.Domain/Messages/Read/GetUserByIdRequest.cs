using Onefocus.Common.Abstractions.Messages;

namespace Onefocus.Wallet.Domain.Messages.Read;

public sealed record GetUserByIdRequest(Guid Id) : IRequest;