﻿using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Bank;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Read;

public interface IBankReadRepository : IBaseContextRepository
{
    Task<Result<GetAllBanksResponseDto>> GetAllBanksAsync(CancellationToken cancellationToken = default);

    Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default);

}