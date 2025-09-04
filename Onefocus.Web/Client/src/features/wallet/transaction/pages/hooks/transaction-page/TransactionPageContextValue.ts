import { Dispatch } from 'react';

type TransactionPageContextValue = {
    showForm: boolean;
    setShowForm: Dispatch<React.SetStateAction<boolean>>;
};

export default TransactionPageContextValue;