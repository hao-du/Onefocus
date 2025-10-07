import { useEffect, useMemo } from 'react';
import SettingsContextValue from './interfaces/SettingsContextValue';
import { GetSettingsByUserIdResponse, useGetAppSettings } from '../../features/home';
import SettingsContext from './SettingsContext';
import { useLocale } from '../locale';

const SettingsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const { appSettings, isAppSettingsReady, refetchAppSettings } = useGetAppSettings();
    const { setLanguage, setLocale, setTimeZone } = useLocale();

    useEffect(() => {
        if (appSettings) {
            if (appSettings.language) setLanguage(appSettings.language);
            if (appSettings.locale) setLocale(appSettings.locale);
            if (appSettings.timeZone) setTimeZone(appSettings.timeZone);
        }
    }, [appSettings, setLanguage, setLocale, setTimeZone])

    const contextValue: SettingsContextValue<GetSettingsByUserIdResponse, unknown> = useMemo(() => ({
        settings: isAppSettingsReady ? appSettings : undefined,
        isSettingsReady: isAppSettingsReady,
        refetch: refetchAppSettings,
    }), [appSettings, refetchAppSettings, isAppSettingsReady]);

    return (
        <SettingsContext.Provider value={contextValue}>
            {children}
        </SettingsContext.Provider>
    );
};

export default SettingsProvider;