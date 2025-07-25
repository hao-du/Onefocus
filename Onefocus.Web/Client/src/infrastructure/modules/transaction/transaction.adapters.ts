import {GetAllTransactionsResponse, GetCashFlowByTransactionIdResponse, TransactionResponse} from './transaction.interfaces';
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
    const toCashFlowEntity = (response: GetCashFlowByTransactionIdResponse): CashFlow => {
        return {
            id : response.id,
            transactionId: response.transactionId,
            transactedOn : response.transactedOn,
            amount : response.amount,
            currencyId: response.currencyId,
            isIncome : response.isIncome,
            description : response.description,
            isActive : response.isActive,
            transactionItems : response.transactionItems.map(item => ({
                id: item.id,
                name: item.name,
                amount: item.amount,
                isActive: item.isActive,
                description: item.description
            }))
        };
    }

    return { toCashFlowEntity };
}


