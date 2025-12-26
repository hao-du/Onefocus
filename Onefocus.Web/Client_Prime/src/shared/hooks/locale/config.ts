import i18n from "i18next";
import { initReactI18next } from "react-i18next";

import { DEFAULT_LANGUAGE, VIETNAMESE_LANGUAGE } from "../constants/Locale";
import { en, vi } from "./languages";
import { addLocale } from "primereact/api";

//Add locales to PrimeReact
addLocale(DEFAULT_LANGUAGE, en);
addLocale(VIETNAMESE_LANGUAGE, vi);

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