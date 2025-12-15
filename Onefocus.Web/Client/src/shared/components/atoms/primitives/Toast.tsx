import { Toast as PrimeToast } from 'primereact/toast';
import { BaseHtmlProps, BaseIdentityProps, BaseStyleProps } from '../../../props/BaseProps';

type ToastRef = {
    show: (message: ToastMessage | ToastMessage[]) => void;
};

interface ToastMessage {
    severity?: 'success' | 'info' | 'warn' | 'error' | 'secondary' | 'contrast' | undefined;
    summary?: React.ReactNode | undefined;
    detail?: React.ReactNode | undefined;
    life?: number;
};

interface ToastProps extends BaseHtmlProps, BaseIdentityProps, BaseStyleProps {
    position?: 'center' | 'top-center' | 'top-left' | 'top-right' | 'bottom-center' | 'bottom-left' | 'bottom-right';
    ref: React.RefObject<ToastRef | null>;
};

export const Toast = (props: ToastProps) => {
    return (
        <PrimeToast
            className={props.className}
            style={props.style}
            ref={props.ref as React.RefObject<PrimeToast>}
            position={props.position ?? 'bottom-right'}
        />
    );
};