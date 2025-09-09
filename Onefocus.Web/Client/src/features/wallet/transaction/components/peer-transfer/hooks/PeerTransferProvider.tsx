import { PropsWithChildren, useCallback } from 'react';
import { useWindows } from '../../../../../../shared/components/hooks';
import { useCreatePeerTransfer, useGetPeerTransferByTransactionId, useUpdatePeerTransfer } from '../../../services';
import { useTransactionList } from '../../transaction-list';
import PeerTransferFormInput from '../interfaces/PeerTransferFormInput';
import PeerTransferContext from './PeerTransferContext';
import { useTransactionPage } from '../../../pages/hooks';

type PeerTransferProviderProps = PropsWithChildren;

const PeerTransferProvider = (props: PeerTransferProviderProps) => {
    const { setShowForm } = useTransactionPage();
    const { refetchList } = useTransactionList();
    const { showResponseToast } = useWindows();

    const { peerTransferEntity: selectedPeerTransfer, isPeerTransferLoading, setPeerTransferTransactionId } = useGetPeerTransferByTransactionId();
    const { onCreateAsync, isCreating } = useCreatePeerTransfer();
    const { onUpdateAsync, isUpdating } = useUpdatePeerTransfer();

    const onPeerTransferSubmit = useCallback(async (data: PeerTransferFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                status: data.status,
                type: data.type,
                counterpartyId: data.counterpartyId,
                description: data.description,
                transferTransactions: data.transferTransactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn,
                    amount: item.amount,
                    currencyId: item.currencyId,
                    description: item.description,
                    isInFlow: item.isInFlow,
                    isActive: item.isActive,
                }))
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setPeerTransferTransactionId(response.value.id);
            }
        } else {
            const response = await onUpdateAsync({
                id: data.id,
                status: data.status,
                type: data.type,
                counterpartyId: data.counterpartyId,
                description: data.description,
                isActive: data.isActive,
                transferTransactions: data.transferTransactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn,
                    amount: item.amount,
                    currencyId: item.currencyId,
                    description: item.description,
                    isInFlow: item.isInFlow,
                    isActive: item.isActive,
                }))
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                setShowForm(false);
            }
        }
        refetchList();
    }, [onCreateAsync, onUpdateAsync, setShowForm, refetchList, setPeerTransferTransactionId, showResponseToast]);

    // Provide the cash flow context to the children components
    return (
        <PeerTransferContext.Provider value={{
            selectedPeerTransfer: selectedPeerTransfer,
            isPeerTransferLoading: isPeerTransferLoading || isCreating || isUpdating,
            setPeerTransferTransactionId: setPeerTransferTransactionId,
            onPeerTransferSubmit: onPeerTransferSubmit,
        }}>
            {props.children}
        </PeerTransferContext.Provider>
    );
};

export default PeerTransferProvider;