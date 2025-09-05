import { useContext } from "react";
import CurrencyExchangeContext from "./CurrencyExchangeContext";

const useCurrencyExchange = () => {
    const context = useContext(CurrencyExchangeContext);
    if (!context) {
        throw new Error('useCurrencyExchange must be used within the CurrencyExchangeProvider');
    }
    return context;
};

export default useCurrencyExchange;