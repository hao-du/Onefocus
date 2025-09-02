import { RefetchOptions } from '@tanstack/react-query';
import { CurrencyResponse } from '../../../../currency';
import { TransactionResponse } from '../../../apis';
import { Dispatch } from 'react';

type TransactionContextValue = {
    transactions:  TransactionResponse[];
    currencies: CurrencyResponse[];
    isListLoading: boolean;
    refetchList: (options?: RefetchOptions) => void
    showForm: boolean;
    setShowForm: Dispatch<React.SetStateAction<boolean>>;
};

export default TransactionContextValue;