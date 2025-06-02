import {BankResponse, GetAllBanksResponse, GetBankByIdResponse} from './bank.interfaces';
import {Bank} from '../../../domain/bank';

export const getAllBanksAdapter = () => {
    const toBankEntity = (response: BankResponse): Bank => {
        return {
            id: response.id,
            name: response.name,
            isActive: response.isActive,
            description: response.description,
            actionedOn: response.actionedOn,
            actionedBy: response.actionedBy,
        };
    }

    const toBankEntities = (response: GetAllBanksResponse): Bank[] => {
        return response.banks.map(bank => toBankEntity(bank));
    }

    return { toBankEntities };
}

export const getBankByIdAdapter = () => {
    const toBankEntity = (response: GetBankByIdResponse): Bank => {
        return {
            id: response.id,
            name: response.name,
            isActive: response.isActive,
            description: response.description,
            actionedOn: response.actionedOn,
            actionedBy: response.actionedBy,
        };
    }

    return { toBankEntity };
}

