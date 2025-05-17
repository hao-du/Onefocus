import {CurrencyResponse, GetAllCurrenciesResponse, GetCurrencyByIdResponse} from './currency.interfaces';
import {Currency} from '../../../domain/currency';

export const getAllCurrenciesAdapter = () => {
    const toCurrencyEntity = (response: CurrencyResponse): Currency => {
        return {
            id: response.id,
            name: response.name,
            shortName: response.shortName,
            defaultFlag: response.defaultFlag,
            activeFlag: response.activeFlag,
            description: response.description,
            actionedOn: response.actionedOn,
            actionedBy: response.actionedBy,
        };
    }

    const toCurrencyEntities = (response: GetAllCurrenciesResponse): Currency[] => {
        return response.currencies.map(currency => toCurrencyEntity(currency));
    }

    return { toCurrencyEntities };
}

export const getCurrencyByIdAdapter = () => {
    const toCurrencyEntity = (response: GetCurrencyByIdResponse): Currency => {
        return {
            id: response.id,
            name: response.name,
            shortName: response.shortName,
            defaultFlag: response.defaultFlag,
            activeFlag: response.activeFlag,
            description: response.description,
            actionedOn: response.actionedOn,
            actionedBy: response.actionedBy,
        };
    }

    return { toCurrencyEntity };
}

