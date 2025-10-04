import { useCallback, useMemo } from 'react';
import SettingsContextValue from './interfaces/SettingsContextValue';
import { QueryObserverResult } from '@tanstack/react-query';
import { GetSettingsByUserIdResponse, useGetAppSettings } from '../../features/home';
import SettingsContext from './SettingsContext';
import { DateTime } from "luxon";
import { findIana } from "windows-iana";
import { normalizeDateTimeString } from '../../utils';

const SettingsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const { appSettings, isAppSettingsReady, refetchAppSettings } = useGetAppSettings();

    const formatDateTime = useCallback((date: Date | string | number, showTime: boolean) => {
        if (!date) return "";
        const rawDate = date instanceof Date ? date : new Date(date);
        if (isNaN(rawDate.getTime())) return ""; // Invalid date

        const ianaZones = findIana(appSettings?.timeZone ?? "UTC");
        let output = '';
        if (ianaZones && ianaZones.length > 0) {
            output = DateTime.fromJSDate(rawDate).setZone(ianaZones[0]).setLocale(appSettings?.locale ?? navigator.language).toLocaleString(DateTime.DATETIME_SHORT);
        } else {
            output = rawDate.toLocaleString(navigator.language, DateTime.DATETIME_SHORT);
        }
        return normalizeDateTimeString(output, showTime);
    }, [appSettings?.locale, appSettings?.timeZone]);

    const contextValue: SettingsContextValue<GetSettingsByUserIdResponse, unknown> = useMemo(() => ({
        settings: isAppSettingsReady ? appSettings : undefined,
        isSettingsReady: isAppSettingsReady,
        refetch: isAppSettingsReady ? refetchAppSettings : async () => Promise.resolve({} as QueryObserverResult<GetSettingsByUserIdResponse, unknown>),
        formatDateTime: formatDateTime
    }), [appSettings, refetchAppSettings, isAppSettingsReady, formatDateTime]);

    return (
        <SettingsContext.Provider value={contextValue}>
            {children}
        </SettingsContext.Provider>
    );
};

export default SettingsProvider;