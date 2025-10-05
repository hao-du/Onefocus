import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { PrimeReactProvider } from "primereact/api";
import { PropsWithChildren } from "react";
import { BrowserRouter } from "react-router";
import { ErrorHandlerProvider, WindowsProvider } from "../components/hooks";
import { ClientProvider, TokenProvider } from "../hooks";
import SettingsProvider from "../hooks/settings/SettingsProvider";
import LocaleProvider from "../hooks/locale/LocaleProvider";

type MainProps = PropsWithChildren & {
    queryClient: QueryClient;
    unauthorizedCallback?: () => Promise<string>;
};

const Main = (props: MainProps) => {
    return (
        <BrowserRouter>
            <WindowsProvider>
                <ErrorHandlerProvider>
                    <PrimeReactProvider>
                        <QueryClientProvider client={props.queryClient}>
                            <TokenProvider>
                                <ClientProvider unauthorizedCallback={props.unauthorizedCallback}>
                                    <SettingsProvider>
                                        <LocaleProvider>
                                            {props.children}
                                        </LocaleProvider>
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