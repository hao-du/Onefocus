import type { cssClassDefinitions } from "./CssClassDefinitions";

export default interface ThemeContextValue {
    cssClasses: typeof cssClassDefinitions
}