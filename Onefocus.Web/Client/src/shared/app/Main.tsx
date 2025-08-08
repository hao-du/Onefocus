import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { PrimeReactProvider } from "primereact/api";
import { PropsWithChildren } from "react";
import { BrowserRouter } from "react-router";
import ErrorHandler from "../components/error/ErrorHandler";
import { WindowsProvider } from "../components/hooks";
import { ClientProvider, TokenProvider } from "../hooks";

type MainProps = PropsWithChildren & {
    queryClient: QueryClient;
    unauthorizedCallback?: () => Promise<string>;
};

const Main = (props: MainProps) => {
    return (
        <BrowserRouter>
            <WindowsProvider>
                <ErrorHandler>
                <PrimeReactProvider>
                    <QueryClientProvider client={props.queryClient}>
                        <TokenProvider>
                            <ClientProvider unauthorizedCallback={props.unauthorizedCallback}>
                                {props.children}
                            </ClientProvider>
                        </TokenProvider>
                    </QueryClientProvider>
                </PrimeReactProvider>
                </ErrorHandler>
            </WindowsProvider>
        </BrowserRouter>
    );
};

export default Main;