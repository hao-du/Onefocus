import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { PrimeReactProvider } from "primereact/api";
import { PropsWithChildren } from "react";
import { BrowserRouter } from "react-router";
import { ErrorHandlerProvider, WindowsProvider } from "../components/hooks";
import SettingsProvider from "../hooks/settings/SettingsProvider";
import LocaleProvider from "../hooks/locale/LocaleProvider";
import { AuthProvider } from "../hooks";

type MainProps = PropsWithChildren & {
    queryClient: QueryClient;
    unauthorizedCallback?: () => Promise<string>;
};

const Main = (props: MainProps) => {
    return (
        <BrowserRouter>
            <LocaleProvider>
                <WindowsProvider>
                    <ErrorHandlerProvider>
                        <PrimeReactProvider>
                            <QueryClientProvider client={props.queryClient}>
                                <AuthProvider>
                                    <SettingsProvider>
                                        {props.children}
                                    </SettingsProvider>
                                </AuthProvider>
                            </QueryClientProvider>
                        </PrimeReactProvider>
                    </ErrorHandlerProvider>
                </WindowsProvider>
            </LocaleProvider>
        </BrowserRouter>
    );
};

export default Main;