import {MenuItem, MenuItemCommandEvent} from 'primereact/menuitem';

export default interface ActionItem extends MenuItem {
    label?: string;
    icon?: string;

    command?(event?: MenuItemCommandEvent): void;
}