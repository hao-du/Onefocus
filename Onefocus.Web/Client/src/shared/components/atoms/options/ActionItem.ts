import { BaseIdentityProps } from '../../../props/BaseProps';

export interface ActionItem extends BaseIdentityProps {
    label?: string;
    icon?: string;
    command?: () => void;
    items?: ActionItem[];
    severity?: 'secondary' | 'success' | 'info' | 'warning' | 'danger' | 'help' | 'contrast';
};