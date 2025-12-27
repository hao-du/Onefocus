import i18n from "i18next";
import { initReactI18next } from "react-i18next";

import { DEFAULT_LANGUAGE } from "../../constants";
import en from "./languages/en";
import vi from "./languages/vi";

//Initialize i18n language translation
i18n
    .use(initReactI18next)
    .init({
        resources: {
            en: { translation: en },
            vi: { translation: vi }
        },
        lng: DEFAULT_LANGUAGE,
        fallbackLng: DEFAULT_LANGUAGE,
        interpolation: {
            escapeValue: false
        },
        keySeparator: false
    });

export default i18n;