import TransactionSearchFormInput from "../interfaces/TransactionSearchFormInput";

type TransactionSearchContextValue = {
    isSearching: boolean;
    onSearch: (data: TransactionSearchFormInput) => void;
};

export default TransactionSearchContextValue;