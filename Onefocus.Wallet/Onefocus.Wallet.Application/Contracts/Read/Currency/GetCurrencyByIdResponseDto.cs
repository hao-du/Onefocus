﻿using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Currency;

public sealed record GetCurrencyByIdResponseDto(Entity.Currency? Currency);