﻿using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.CashFlow;

public sealed record UpdateCurrencyRequestDto(Entity.Transaction Transaction);
