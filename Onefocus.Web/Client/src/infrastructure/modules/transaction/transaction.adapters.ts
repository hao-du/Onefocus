import {GetAllTransactionsResponse, GetCashFlowByIdResponse, TransactionResponse} from './transaction.interfaces';
import {Transaction} from '../../../domain/transaction';
import {CashFlow} from '../../../domain/transactions/cashFlow';

export const getAllTransactionsAdapter = () => {
    const toTransactionEntity = (response: TransactionResponse): Transaction => {
        return {
            id : response.id,
            transactedOn : response.transactedOn,
            amount : response.amount,
            currencyName : response.currencyName,
            description : response.description,
            type : response.type,
            tags : response.tags
        };
    }

    const toTransactionEntities = (response: GetAllTransactionsResponse): Transaction[] => {
        return response.transactions.map(transaction => toTransactionEntity(transaction));
    }

    return { toTransactionEntities };
}

export const getCashFlowByIdAdapter = () => {
    const toCashFlowEntity = (response: GetCashFlowByIdResponse): CashFlow => {
        return {
            id : response.id,
            transactedOn : response.transactedOn,
            amount : response.amount,
            currencyId: response.currencyId,
            isIncome : response.isIncome,
            description : response.description,
            isActive : response.isActive
        };
    }

    return { toCashFlowEntity };
}


