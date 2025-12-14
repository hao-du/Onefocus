import useGetAllTransactions from "./useGetAllTransactions";

import useCreateCashFlow from "./cashflow/useCreateCashFlow";
import useUpdateCashFlow from "./cashflow/useUpdateCashFlow";
import useGetCashFlowByTransactionId from "./cashflow/useGetCashFlowByTransactionId";

import useCreateBankAccount from "./bank-account/useCreateBankAccount";
import useUpdateBankAccount from "./bank-account/useUpdateBankAccount";
import useGetBankAccountByTransactionId from "./bank-account/useGetBankAccountByTransactionId";

import useCreateCurrencyExchange from "./currency-exchange/useCreateCurrencyExchange";
import useUpdateCurrencyExchange from "./currency-exchange/useUpdateCurrencyExchange";
import useGetCurrencyExchangeByTransactionId from "./currency-exchange/useGetCurrencyExchangeByTransactionId";

import useCreatePeerTransfer from "./peer-transfer/useCreatePeerTransfer";
import useUpdatePeerTransfer from "./peer-transfer/useUpdatePeerTransfer";
import useGetPeerTransferByTransactionId from "./peer-transfer/useGetPeerTransferByTransactionId";

import useSearchTransactions from "./search/useSearchTransactions";
import SearchCriteria from "./search/interfaces/SearchCriteria";

export {
    useGetAllTransactions,
    useSearchTransactions, useCreateCashFlow,
    useUpdateCashFlow,
    useGetCashFlowByTransactionId,

    useCreateBankAccount,
    useUpdateBankAccount,
    useGetBankAccountByTransactionId,

    useCreateCurrencyExchange,
    useUpdateCurrencyExchange,
    useGetCurrencyExchangeByTransactionId,

    useCreatePeerTransfer,
    useUpdatePeerTransfer,
    useGetPeerTransferByTransactionId
};
export type { SearchCriteria };

