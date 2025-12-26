import { RefetchOptions } from '@tanstack/react-query';
import { CurrencyResponse } from '../../../../currency';
import { TransactionResponse } from '../../../apis';
import { BankResponse } from '../../../../bank';
import { CounterpartyResponse } from '../../../../counterparty';

type TransactionListContextValue = {
    transactions:  TransactionResponse[];
    currencies: CurrencyResponse[];
    counterparties: CounterpartyResponse[];
    banks: BankResponse[];
    isListLoading: boolean;
    refetchList: (options?: RefetchOptions) => void
};

export default TransactionListContextValue;