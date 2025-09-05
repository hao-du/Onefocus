import { createContext } from 'react';
import CurrencyExchangeContextValue from './CurrencyExchangeContextValue';

const CurrencyExchangeContext = createContext<CurrencyExchangeContextValue | null>(null);

export default CurrencyExchangeContext;