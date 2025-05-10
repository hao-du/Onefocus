using Microsoft.AspNetCore.Http;
using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Messages.Write;
using Onefocus.Wallet.Domain.Repositories.Read;
using Onefocus.Wallet.Domain.Repositories.Write;
using Onefocus.Wallet.Domain.Specifications.Currency;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Pipelines;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Onefocus.Membership.Application.User.Commands;
public sealed record CreateCurrencyCommandRequest(string Name, string ShortName, bool DefaultFlag, string? Description) : ICommand
{
    public CreateCurrencyRequestDto CastToDto(Guid createdBy) => new(Name, ShortName, DefaultFlag, Description, createdBy);
}

internal sealed class CreateCurrencyCommandHandler : ICommandHandler<CreateCurrencyCommandRequest>
{
    private readonly ICurrencyReadRepository _readRepository;
    private readonly ICurrencyWriteRepository _writeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCurrencyCommandHandler(
        ICurrencyReadRepository readRepository
        , ICurrencyWriteRepository writeRepository
        , IHttpContextAccessor httpContextAccessor
    )
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure) return Result.Failure(validationResult.Error);

        if (!Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid createdBy))
        {
            return Result.Failure(CommonErrors.UserClaimInvalid);
        }

        var repoResult = await _writeRepository.CreateCurrencyAsync(request.CastToDto(createdBy), cancellationToken);
        if (repoResult.IsFailure) return Result.Failure(repoResult.Error);

        return Result.Success();
    }

    private async Task<Result> ValidateRequest(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name)) return Result.Failure(Errors.Currency.NameRequired);
        if (string.IsNullOrEmpty(request.ShortName)) return Result.Failure(Errors.Currency.ShortNameRequired);
        if (request.ShortName.Length < 3 || request.ShortName.Length > 4) return Result.Failure(Errors.Currency.ShortNameLengthMustBeThreeOrFour);

        var spec = FindNameSpecification.Create(request.Name).Or(FindShortNameSpecification.Create(request.ShortName));
        var queryResult = await _readRepository.GetCurrencyBySpecificationAsync(new(spec), cancellationToken);
        if (queryResult.IsFailure)
        {
            return Result.Failure(queryResult.Error);
        }
        if (queryResult.Value.Currencies.Count > 0)
        {
            return Result.Failure(Errors.Currency.NameOrShortNameIsExisted);
        }

        return Result.Success();
    }
}

