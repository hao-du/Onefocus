import { PropsWithChildren, useState } from 'react';
import TransactionContext from './TransactionPageContext';

type TransactionPageProviderProps = PropsWithChildren;

const TransactionPageProvider = (props: TransactionPageProviderProps) => {
    const [ showForm, setShowForm] = useState(false);

    return (
        <TransactionContext.Provider value={{
            showForm: showForm,
            setShowForm: setShowForm
        }}>
            {props.children}
        </TransactionContext.Provider>
    );
};

export default TransactionPageProvider;