import { PropsWithChildren, useCallback } from 'react';
import { useWindows } from '../../../../../../shared/components/hooks';
import { useTransactionList } from '../../transaction-list';
import BankAccountFormInput from '../interfaces/BankAccountFormInput';
import BankAccountContext from './BankAccountContext';
import { useTransactionPage } from '../../../pages/hooks';
import { useCreateBankAccount, useGetBankAccountByTransactionId, useUpdateBankAccount } from '../../../services';

type BankAccountProviderProps = PropsWithChildren;

const BankAccountProvider = (props: BankAccountProviderProps) => {
    const { setShowForm } = useTransactionPage();
    const { refetchList } = useTransactionList();
    const { showResponseToast } = useWindows();

    const { bankAccountEntity: selectedBankAccount, isBankAccountLoading, setBankAccountTransactionId } = useGetBankAccountByTransactionId();
    const { onCreateAsync, isCreating } = useCreateBankAccount();
    const { onUpdateAsync, isUpdating } = useUpdateBankAccount();

    const onBankAccountSubmit = useCallback(async (data: BankAccountFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                amount: data.amount,
                currencyId: data.currencyId,
                interestRate: data.interestRate,
                accountNumber: data.accountNumber,
                issuedOn: data.issuedOn,
                closedOn: data.closedOn,
                isClosed: data.isClosed,
                bankId: data.bankId,
                description: data.description,
                transactions: data.transactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn,
                    currencyId: item.currencyId,
                    amount: item.amount,
                    isActive: item.isActive,
                    description: item.description
                }))
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setBankAccountTransactionId(response.value.id);
            }
        } else {
            const response = await onUpdateAsync({
                id: data.id,
                amount: data.amount,
                currencyId: data.currencyId,
                interestRate: data.interestRate,
                accountNumber: data.accountNumber,
                issuedOn: data.issuedOn,
                closedOn: data.closedOn,
                isClosed: data.isClosed,
                bankId: data.bankId,
                description: data.description,
                isActive: data.isActive,
                transactions: data.transactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn,
                    currencyId: item.currencyId,
                    amount: item.amount,
                    isActive: item.isActive,
                    description: item.description
                }))
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                setShowForm(false);
            }
        }
        refetchList();
    }, [onCreateAsync, onUpdateAsync, setShowForm, refetchList, setBankAccountTransactionId, showResponseToast]);

    // Provide the cash flow context to the children components
    return (
        <BankAccountContext.Provider value={{
            selectedBankAccount: selectedBankAccount,
            isBankAccountLoading: isBankAccountLoading || isCreating || isUpdating,
            setBankAccountTransactionId: setBankAccountTransactionId,
            onBankAccountSubmit: onBankAccountSubmit,
        }}>
            {props.children}
        </BankAccountContext.Provider>
    );
};

export default BankAccountProvider;