import { DateTime } from "luxon";
import { useCallback, useEffect, useMemo } from "react";
import { findIana } from "windows-iana";
import { normalizeDateTimeString } from "../../utils";
import { useSettings } from "../settings";
import LocaleContext from "./LocaleContext";
import LocaleContextValue from "./interfaces/LocaleValueContext";
import { DEFAULT_LANGUAGE, DEFAULT_LOCALE, DEFAULT_TIMEZONE } from "../constants/Locale";
import { locale } from "primereact/api";
import { useTranslation } from "react-i18next";

const LocaleProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const { settings } = useSettings();
    const { t, i18n } = useTranslation();

    useEffect(() => {
        if (settings?.language) {
            locale(settings.language);
            i18n.changeLanguage(settings.language)
        }
    }, [settings?.language, i18n]);

    const formatDateTime = useCallback((date: Date | string | number, showTime: boolean) => {
        if (!date) return "";
        const rawDate = date instanceof Date ? date : new Date(date);
        if (isNaN(rawDate.getTime())) return ""; // Invalid date

        const ianaZones = findIana(settings?.timeZone ?? DEFAULT_TIMEZONE);
        let output = '';
        if (ianaZones && ianaZones.length > 0) {
            output = DateTime.fromJSDate(rawDate).setZone(ianaZones[0]).setLocale(settings?.locale ?? DEFAULT_LOCALE).toLocaleString(DateTime.DATETIME_SHORT);
        } else {
            output = rawDate.toLocaleString(navigator.language, DateTime.DATETIME_SHORT);
        }
        return normalizeDateTimeString(output, showTime);
    }, [settings?.locale, settings?.timeZone]);

    const translate = useCallback((input?: string) => {
        const value = input ?? '';
        if (settings?.language) {
            return t(value);
        }
        return value;
    }, [settings?.language, t]);

    const contextValue: LocaleContextValue = useMemo(() => ({
        language: settings?.language ?? DEFAULT_LANGUAGE,
        locale: settings?.locale ?? DEFAULT_LOCALE,
        timeZone: settings?.timeZone ?? DEFAULT_TIMEZONE,
        formatDateTime: formatDateTime,
        translate: translate
    }), [settings?.language, settings?.locale, settings?.timeZone, formatDateTime, translate]);

    return (
        <LocaleContext.Provider value={contextValue}>
            {children}
        </LocaleContext.Provider>
    );
};

export default LocaleProvider;