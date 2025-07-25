import {Currency} from '../../../../../../domain/currency';
import {RefetchOptions} from '@tanstack/react-query';
import {Transaction} from '../../../../../../domain/transaction';

export type TransactionContextValue = {
    transactions:  Transaction[];
    currencies: Currency[];
    isListLoading: boolean;
    refetchList: (options?: RefetchOptions) => void
};