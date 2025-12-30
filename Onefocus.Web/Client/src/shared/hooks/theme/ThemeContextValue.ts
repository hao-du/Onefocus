import type { cssClassDefinitions } from "./CssClassDefinitions";
import StyleDefinitions from "./StyleDefinitions";

export default interface ThemeContextValue {
    cssClasses: typeof cssClassDefinitions;
    styles: StyleDefinitions;
}