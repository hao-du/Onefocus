import { SizeType } from "./types";

export const APP_NAME = "Onefocus";
export const MOBILE_BREAK_POINT = 768;
export const DEFAULT_LOCALE = "en-US";
export const DEFAULT_LANGUAGE = "en";
export const DEFAULT_TIMEZONE = "UTC";
export const VIETNAMESE_LANGUAGE = "vi";
export const CHECK_ACCESS_TOKEN_INTERVAL_MILLISECOND = 60_000;
export const GRID_COLUMNS = 24;
export const DRAWER_DEFAULT_WIDTH = 400;
export const NUMBER_BY_SIZE_TYPE: Record<SizeType | 'xlarge', number> = {
    small: 4,
    middle: 6,
    large: 8,
    xlarge: 10,
}