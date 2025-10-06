import { PropsWithChildren, useCallback, useEffect, useRef, useState } from 'react';
import { ApiResponseBase, useLocale } from '../../../hooks';
import { Toast, ToastMessage, ToastRef } from '../../indicators';
import WindowsContext from './WindowsContext';

type WindowsProviderProps = PropsWithChildren & {};

const WindowsProvider = (props: WindowsProviderProps) => {
    const {translate} = useLocale();
    const [isMobile, setIsMobile] = useState(() => window.innerWidth < 768);
    const toastRef = useRef<ToastRef>(null);

    const showToast = useCallback((message: ToastMessage | ToastMessage[]) => {
        const mapSeverityToSummary = (message: ToastMessage) => {
            if (!message.summary) {
                const severity = message.severity;
                message.summary = translate(severity ? severity[0].toUpperCase() + severity.slice(1) : severity);
            }
        }
        if (Array.isArray(message)) {
            message.map((c) => mapSeverityToSummary(c));
        } else {
            mapSeverityToSummary(message);
        }
        toastRef.current?.show(message);
    }, []);

    const showResponseToast = useCallback((response: ApiResponseBase, message?: string) => {
        if (response.status >= 200 && response.status < 300) {
            if(!message) return;
            showToast({
                severity: 'success',
                detail: translate(message),
            });
        } else if (response.errors) {
            showToast(response.errors?.map((error): ToastMessage => {
                return {
                    severity: 'error',
                    detail: translate(error.description),
                    summary: translate(`Server error`),
                };
            }));
        } else {
            showToast({
                severity: 'error',
                detail: translate('Oops! Something went wrong.'),
            });
        }
    }, [showToast, translate]);

    useEffect(() => {
        const handleResize = () => {
            if (window.innerWidth < 768) {
                setIsMobile(true);
            }
            else {
                setIsMobile(false);
            }
        }
        window.addEventListener('resize', handleResize);

        return () => window.removeEventListener('resize', handleResize);
    }, []);

    return (
        <WindowsContext.Provider value={{
            isMobile: isMobile,
            showToast: showToast,
            showResponseToast: showResponseToast
        }}>
            <Toast ref={toastRef}/>
            {props.children}
        </WindowsContext.Provider>
    );
};

export default WindowsProvider;