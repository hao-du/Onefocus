import { ReactNode } from "react";
import { NavigableMenuItem } from "./NavigableMenuItem";

export interface MenuOption extends Omit<NavigableMenuItem, 'url'> {
    icon?: ReactNode;
    type?: 'group' | 'divider';
    children?: MenuOption[];
    url?: string;
}