import { createContext } from 'react';
import SettingsContextValue from './interfaces/SettingsContextValue';
import { QueryObserverResult } from '@tanstack/react-query';
import { GetSettingsByUserIdResponse } from '../../features/home';

const SettingsContext = createContext<SettingsContextValue<GetSettingsByUserIdResponse, unknown>>({
    settings: undefined,
    isSettingsReady: false,
    refetch: async () => Promise.resolve({} as QueryObserverResult<GetSettingsByUserIdResponse, unknown>)
});

export default SettingsContext;