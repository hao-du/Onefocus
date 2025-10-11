using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Identity.Application.Interfaces.Repositories;
using Onefocus.Identity.Application.Interfaces.Services;

namespace Onefocus.Identity.Application.UseCases.Authentication.Commands;

public sealed record LogOutCommandRequest() : ICommand;

internal sealed class LogOutCommandHandler(
    ILogger<AuthenticateCommandHandler> logger,
    IHttpContextAccessor httpContextAccessor
) : CommandHandler<LogOutCommandRequest>(httpContextAccessor, logger)
{
    public override Task<Result> Handle(LogOutCommandRequest request, CancellationToken cancellationToken = default)
    {
        DeleteCookie("r");
        DeleteCookie("i");

        return Task.FromResult(Result.Success());
    }
}

