import { createContext } from 'react';
import PeerTransferContextValue from './PeerTransferContextValue';

const PeerTransferContext = createContext<PeerTransferContextValue | null>(null);

export default PeerTransferContext;