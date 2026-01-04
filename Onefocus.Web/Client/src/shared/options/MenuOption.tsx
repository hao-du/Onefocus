import { ReactNode } from "react";
import { NavigableOption } from "./NavigableOption";

export interface MenuOption extends Omit<NavigableOption, 'url'> {
    icon?: ReactNode;
    type?: 'group' | 'divider';
    children?: MenuOption[];
    url?: string;
}