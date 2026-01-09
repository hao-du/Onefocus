/* eslint-disable @typescript-eslint/no-empty-object-type */
import { BrowserRouter } from "react-router";
import { ChildrenProps } from "../props/BaseProps";
import ClientProvider from "../hooks/client/ClientProvider";
import AuthProvider from "../hooks/auth/AuthProvider";
import SettingsProvider from "../hooks/settings/SettingsProvider";
import WindowsProvider from "../hooks/windows/WindowsProvider";
import ErrorHandlerProvider from "../hooks/error/ErrorHandlerProvider";
import LocaleProvider from "../hooks/locale/LocaleProvider";
import ThemeProvider from "../hooks/theme/ThemeProvider";

interface MainProps extends ChildrenProps {
};

const Main = (props: MainProps) => {
    return (
        <BrowserRouter>
            <ClientProvider>
                <LocaleProvider>
                    <ThemeProvider>
                        <WindowsProvider>
                            <ErrorHandlerProvider>
                                <AuthProvider>
                                    <SettingsProvider>
                                        {props.children}
                                    </SettingsProvider>
                                </AuthProvider>
                            </ErrorHandlerProvider>
                        </WindowsProvider>
                    </ThemeProvider>
                </LocaleProvider>
            </ClientProvider>
        </BrowserRouter>
    );
};

export default Main;