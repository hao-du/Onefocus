/* eslint-disable @typescript-eslint/no-explicit-any */

import { createContext } from "react";
import PageContextValue from "./PageContextValue";

const PageContext = createContext<PageContextValue<any> | null>(null);

export default PageContext;