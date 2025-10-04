import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { addLocale, PrimeReactProvider } from "primereact/api";
import { PropsWithChildren } from "react";
import { BrowserRouter } from "react-router";
import { ErrorHandlerProvider, WindowsProvider } from "../components/hooks";
import { ClientProvider, TokenProvider } from "../hooks";
import SettingsProvider from "../hooks/settings/SettingsProvider";

type MainProps = PropsWithChildren & {
    queryClient: QueryClient;
    unauthorizedCallback?: () => Promise<string>;
};

const Main = (props: MainProps) => {
    addLocale("vi-VN", {
        firstDayOfWeek: 1,
        dayNames: ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"],
        dayNamesShort: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
        dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
        monthNames: [
            "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
            "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
        ],
        monthNamesShort: ["Th1", "Th2", "Th3", "Th4", "Th5", "Th6", "Th7", "Th8", "Th9", "Th10", "Th11", "Th12"],
        today: "Hôm nay",
        clear: "Xóa",
        dateFormat: "dd/mm/yy",
    });

    addLocale('en-US', {
        firstDayOfWeek: 0,
        dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        monthNames: [
            'January', 'February', 'March', 'April', 'May', 'June',
            'July', 'August', 'September', 'October', 'November', 'December'
        ],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                          'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        today: 'Today',
        clear: 'Clear',
        dateFormat: "mm/dd/yy",
    });

    return (
        <BrowserRouter>
            <WindowsProvider>
                <ErrorHandlerProvider>
                    <PrimeReactProvider>
                        <QueryClientProvider client={props.queryClient}>
                            <TokenProvider>
                                <ClientProvider unauthorizedCallback={props.unauthorizedCallback}>
                                    <SettingsProvider>
                                        {props.children}
                                    </SettingsProvider>
                                </ClientProvider>
                            </TokenProvider>
                        </QueryClientProvider>
                    </PrimeReactProvider>
                </ErrorHandlerProvider>
            </WindowsProvider>
        </BrowserRouter>
    );
};

export default Main;