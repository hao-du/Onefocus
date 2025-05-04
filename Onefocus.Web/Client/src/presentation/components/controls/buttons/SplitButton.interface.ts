import {MenuItem, MenuItemCommandEvent} from 'primereact/menuitem';

export interface SplitButtonActionItem extends MenuItem {
    label?: string;
    icon?: string;

    command?(event: MenuItemCommandEvent): void;
}