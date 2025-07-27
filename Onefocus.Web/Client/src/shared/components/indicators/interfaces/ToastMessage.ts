export interface ToastMessage {
    severity?: 'success' | 'info' | 'warn' | 'error' | 'secondary' | 'contrast';
    summary?: React.ReactNode;
    detail?: React.ReactNode;
    life?: number;
}