import { SelectOption } from "../../../shared/options/SelectOption";
import { ColorType } from "../../../shared/types";
import { TRANSACTION_COMPONENT_NAMES } from "../../constants";
import BankResponse from "../apis/interfaces/bank/BankResponse";
import CounterpartyResponse from "../apis/interfaces/counterparty/CounterpartyResponse";
import CurrencyResponse from "../apis/interfaces/currency/CurrencyResponse";

export interface TransactionItemInput {
    id?: string;
    name: string;
    amount: number;
    isActive: boolean;
    description?: string;
}

export interface Meta {
    componentName: string;
    color: ColorType;
}

export enum TransactionType {
    BankAccount = 0,
    PeerTransfer = 1,
    CurrencyExchange = 2,
    CashFlow = 3,
}

export const getComponentName = (type: TransactionType): Meta | undefined => {
    switch (type) {
        case TransactionType.CashFlow:
            return {
                componentName: TRANSACTION_COMPONENT_NAMES.CashFlow,
                color: "green"
            };
        case TransactionType.BankAccount:
            return {
                componentName: TRANSACTION_COMPONENT_NAMES.BankAccount,
                color: "blue"
            };
        case TransactionType.CurrencyExchange:
            return {
                componentName: TRANSACTION_COMPONENT_NAMES.CurrencyExchange,
                color: "gold"
            };
        case TransactionType.PeerTransfer:
            return {
                componentName: TRANSACTION_COMPONENT_NAMES.PeerTransfer,
                color: "purple"
            };
        default:
            return undefined;
    }
};

export const getCurrencyOptions = (currencies: CurrencyResponse[] | undefined): SelectOption[] => {
    if (!currencies) return [];
    return currencies.map((currency) => {
        return {
            label: `${currency.shortName} - ${currency.name}`,
            value: currency.id
        } as SelectOption;
    });
};

export const getBankOptions = (banks: BankResponse[] | undefined): SelectOption[] => {
    if (!banks) return [];
    return banks.map((bank) => {
        return {
            label: bank.name,
            value: bank.id
        } as SelectOption;
    });
};

export const getCounterpartyOptions = (counterparties: CounterpartyResponse[] | undefined): SelectOption[] => {
    if (!counterparties) return [];
    return counterparties.map((counterparty) => {
        return {
            label: counterparty.fullName,
            value: counterparty.id
        } as SelectOption;
    });
};