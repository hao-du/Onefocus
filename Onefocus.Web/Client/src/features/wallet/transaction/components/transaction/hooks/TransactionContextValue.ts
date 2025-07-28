import { RefetchOptions } from '@tanstack/react-query';
import { CurrencyResponse } from '../../../../currency';
import { TransactionResponse } from '../../../apis';

type TransactionContextValue = {
    transactions:  TransactionResponse[];
    currencies: CurrencyResponse[];
    isListLoading: boolean;
    refetchList: (options?: RefetchOptions) => void
};

export default TransactionContextValue;