import { BrowserRouter } from "react-router";
import { WindowsProvider } from "../components/hooks";
import { PrimeReactProvider } from "primereact/api";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ClientProvider, TokenProvider } from "../hooks";
import { PropsWithChildren } from "react";

type MainProps = PropsWithChildren & {
    queryClient: QueryClient;
    unauthorizedCallback?: () => Promise<string>;
};

const Main = (props: MainProps) => {
  return (
    <BrowserRouter>
        <WindowsProvider>
            <PrimeReactProvider>
                <QueryClientProvider client={props.queryClient}>
                    <TokenProvider>
                        <ClientProvider unauthorizedCallback={props.unauthorizedCallback}>
                            {props.children}
                        </ClientProvider>
                    </TokenProvider>
                </QueryClientProvider>
            </PrimeReactProvider>
        </WindowsProvider>
    </BrowserRouter>
  );
};

export default Main;