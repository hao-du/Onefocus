import CurrencyExchangeFormInput from "../interfaces/CurrencyExchangeFormInput";

type CurrencyExchangeContextValue = {
    selectedCurrencyExchange:  CurrencyExchangeFormInput | null | undefined;
    isCurrencyExchangeLoading: boolean;
    setCurrencyExchangeTransactionId: (value: string | null) => void;
    onCurrencyExchangeSubmit: (data: CurrencyExchangeFormInput) => void;
};

export default CurrencyExchangeContextValue;