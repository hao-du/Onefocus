import { createBank, getAllBanks, getBankById, updateBank } from './bankApis';
import { CreateBankRequest, CreateBankResponse, UpdateBankRequest, BankResponse } from './interfaces';

export {
    getAllBanks,
    getBankById,
    createBank,
    updateBank
};
export type {
    CreateBankRequest,
    CreateBankResponse,
    UpdateBankRequest,
    BankResponse
};
