﻿using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Messages.Write;

public sealed record CreateCurrencyRequestDto(Entity.Currency Currency);
