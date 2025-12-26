import PeerTransferFormInput from "../interfaces/PeerTransferFormInput";

type PeerTransferContextValue = {
    selectedPeerTransfer:  PeerTransferFormInput | null | undefined;
    isPeerTransferLoading: boolean;
    setPeerTransferTransactionId: (value: string | null) => void;
    onPeerTransferSubmit: (data: PeerTransferFormInput) => void;
};

export default PeerTransferContextValue;