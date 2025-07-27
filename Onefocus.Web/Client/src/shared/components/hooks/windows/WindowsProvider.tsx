import {PropsWithChildren, useCallback, useEffect, useRef, useState} from 'react';
import {Toast, ToastMessage, ToastRef} from '../../indicators';
import { ApiResponseBase } from '../../../hooks';
import WindowsContext from './WindowsContext';

type WindowsProviderProps = PropsWithChildren & {};

const WindowsProvider = (props: WindowsProviderProps) => {
    const [isMobile, setIsMobile] = useState(() => window.innerWidth < 768);
    const toastRef = useRef<ToastRef>(null);

    const showToast = useCallback((message: ToastMessage | ToastMessage[]) => {
        const mapSeverityToSummary = (message: ToastMessage) => {
            if (!message.summary) {
                const severity = message.severity;
                message.summary = severity ? severity[0].toUpperCase() + severity.slice(1) : severity;
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
                detail: message
            });
        } else if (response.errors) {
            showToast(response.errors?.map((error): ToastMessage => {
                return {
                    severity: 'error',
                    detail: error.description,
                    summary: `Server error`,
                };
            }));
        } else {
            showToast({
                severity: 'error',
                detail: 'Oops! Something went wrong.'
            });
        }
    }, [showToast]);

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