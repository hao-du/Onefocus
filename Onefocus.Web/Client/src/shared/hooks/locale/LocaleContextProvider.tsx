import { useCallback, useEffect, useMemo, useState } from 'react';
import { ChildrenProps } from '../../props/BaseProps';
import { normalizeDateTimeString } from '../../utils/formatUtils';
import { DEFAULT_LANGUAGE, DEFAULT_LOCALE, DEFAULT_TIMEZONE } from './constants';
import { DateTime } from 'luxon';
import { useTranslation } from "react-i18next";
import { findIana } from 'windows-iana';
import { locale as primeLocale } from "primereact/api";
import { LocaleContext, LocaleContextValue } from './LocaleContext';

export const LocaleContextProvider = ({ children }: ChildrenProps) => {
    const { t, i18n } = useTranslation();

    const [language, setLanguage] = useState<string>(() => {
        return localStorage.getItem("language") || DEFAULT_LANGUAGE;
    });
    const [timeZone, setTimeZone] = useState<string>(() => {
        return localStorage.getItem("timeZone") || DEFAULT_TIMEZONE;
    });
    const [locale, setLocale] = useState<string>(() => {
        return localStorage.getItem("locale") || DEFAULT_LOCALE;
    });

    useEffect(() => {
        primeLocale(language);
        i18n.changeLanguage(language);
        localStorage.setItem("language", language);
    }, [language, i18n]);

    useEffect(() => {
        localStorage.setItem("timeZone", timeZone);
    }, [timeZone]);

    useEffect(() => {
        localStorage.setItem("locale", locale);
    }, [locale]);

    const formatDateTime = useCallback((date: Date | string | number, showTime: boolean) => {
        if (!date) return "";
        const rawDate = date instanceof Date ? date : new Date(date);
        if (isNaN(rawDate.getTime())) return ""; // Invalid date

        const ianaZones = findIana(timeZone);
        let output = '';
        if (ianaZones && ianaZones.length > 0) {
            output = DateTime.fromJSDate(rawDate).setZone(ianaZones[0]).setLocale(locale).toLocaleString(DateTime.DATETIME_SHORT);
        } else {
            output = rawDate.toLocaleString(navigator.language, DateTime.DATETIME_SHORT);
        }
        return normalizeDateTimeString(output, showTime);
    }, [locale, timeZone]);

    const translate = useCallback((input?: string) => {
        const value = input ?? '';
        if (language) {
            return t(value);
        }
        return value;
    }, [language, t]);

    const contextValue: LocaleContextValue = useMemo(() => ({
        language: language,
        setLanguage: setLanguage,
        locale: locale,
        setLocale: setLocale,
        timeZone: timeZone,
        setTimeZone: setTimeZone,
        formatDateTime: formatDateTime,
        translate: translate,
    }), [language, locale, timeZone, formatDateTime, translate]);

    return (
        <LocaleContext.Provider value={contextValue}>
            {children}
        </LocaleContext.Provider>
    );
};