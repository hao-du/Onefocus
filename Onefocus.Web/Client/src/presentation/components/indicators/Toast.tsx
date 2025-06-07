import { BaseProps } from '../../props/BaseProps';
import { Toast as PiToast } from 'primereact/toast';

export interface ToastMessage {
    severity?: 'success' | 'info' | 'warn' | 'error' | 'secondary' | 'contrast' | undefined;
    summary?: React.ReactNode | undefined;
    detail?: React.ReactNode | undefined;
    life?: number;
}

export interface ToastRef {
    show: (message: ToastMessage | ToastMessage[]) => void;
}

type ToastProps = BaseProps & {
    position?: 'center' | 'top-center' | 'top-left' | 'top-right' | 'bottom-center' | 'bottom-left' | 'bottom-right';
    ref: React.RefObject<ToastRef | null>;
};

export const Toast = (props: ToastProps) => {
    return (
        <PiToast
            className={props.className}
            style={props.style}
            ref={props.ref as React.RefObject<PiToast>}
            position={props.position ?? 'bottom-right'}
        />
    );
};