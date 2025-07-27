import {createContext} from 'react';
import ClientContextValue from './interfaces/ClientContextValue';
import client from './client';

const ClientContext = createContext<ClientContextValue>({
    client: client,
    isClientReady: false,
});

export default ClientContext;