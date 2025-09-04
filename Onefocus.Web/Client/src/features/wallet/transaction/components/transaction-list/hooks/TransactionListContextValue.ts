import { RefetchOptions } from '@tanstack/react-query';
import { CurrencyResponse } from '../../../../currency';
import { TransactionResponse } from '../../../apis';
import { BankResponse } from '../../../../bank';

type TransactionListContextValue = {
    transactions:  TransactionResponse[];
    currencies: CurrencyResponse[];
    banks: BankResponse[];
    isListLoading: boolean;
    refetchList: (options?: RefetchOptions) => void
};

export default TransactionListContextValue;