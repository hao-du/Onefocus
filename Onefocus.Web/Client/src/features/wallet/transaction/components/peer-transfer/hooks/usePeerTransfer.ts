import { useContext } from "react";
import PeerTransferContext from "./PeerTransferContext";

const usePeerTransfer = () => {
    const context = useContext(PeerTransferContext);
    if (!context) {
        throw new Error('usePeerTransfer must be used within the PeerTransferProvider');
    }
    return context;
};

export default usePeerTransfer;