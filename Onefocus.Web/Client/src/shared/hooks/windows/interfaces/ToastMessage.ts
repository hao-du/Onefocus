import { ReactNode } from "react";

export default interface ToastMessage {
    severity?: 'success' | 'info' | 'warning' | 'error';
    title?: string;
    description?: string;
    icon?: ReactNode;
    canBeExpired?: boolean;
}