import { PropsWithChildren, useCallback, useEffect, useState } from 'react';
import { notification } from 'antd';

import WindowsContext from './WindowsContext';
import useLocale from '../locale/useLocale';
import ToastMessage from './interfaces/ToastMessage';
import ApiResponseBase from '../../apis/interfaces/ApiResponseBase';
import { MOBILE_BREAK_POINT } from '../../constants';

type WindowsProviderProps = PropsWithChildren & {};

const WindowsProvider = (props: WindowsProviderProps) => {
    const { translate } = useLocale();
    const [isMobile, setIsMobile] = useState(() => window.innerWidth < MOBILE_BREAK_POINT);
    const [api, contextHolder] = notification.useNotification();

    const showToast = useCallback((message: ToastMessage | ToastMessage[]) => {
        const mapSeverityToSummary = (message: ToastMessage) => {
            if (!message.description) {
                const severity = message.severity;
                message.description = translate(severity ? severity[0].toUpperCase() + severity.slice(1) : severity);
            }
        }

        const open = (message: ToastMessage) => {
            const castedMessage = message as ToastMessage;
            api[castedMessage.severity ?? 'info']({
                title: castedMessage.title,
                description: castedMessage.description,
                icon: castedMessage.icon,
                showProgress: true,
                pauseOnHover: true,
                placement: 'bottomRight',
            });
        }

        if (Array.isArray(message)) {
            message.forEach((item) => {
                mapSeverityToSummary(item);
                open(item);
            });
            return;
        }

        mapSeverityToSummary(message);
        open(message);

    }, [api, translate]);

    const showResponseToast = useCallback((response: ApiResponseBase, message?: string) => {
        if (response.status >= 200 && response.status < 300) {
            if (!message) return;
            showToast({
                severity: 'success',
                description: translate(message),
            });
        } else if (response.errors) {
            showToast(response.errors?.map((error): ToastMessage => {
                return {
                    severity: 'error',
                    description: translate(error.description),
                    title: translate(`Server error`),
                };
            }));
        } else {
            showToast({
                severity: 'error',
                description: translate('Oops! Something went wrong.'),
            });
        }
    }, [showToast, translate]);

    /**
     * Detect browser's size when resizing Windows.
     */
    useEffect(() => {
        const handleResize = () => {
            if (window.innerWidth < MOBILE_BREAK_POINT) {
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
            {contextHolder}
            {props.children}
        </WindowsContext.Provider>
    );
};

export default WindowsProvider;