import { MenuItem, MenuItemCommandEvent } from 'primereact/menuitem';

export default interface ActionItem extends MenuItem {
    label?: string;
    icon?: string;
    command?(event?: MenuItemCommandEvent): void;
    items?: ActionItem[];
    severity?: 'secondary' | 'success' | 'info' | 'warning' | 'danger' | 'help' | 'contrast';
}