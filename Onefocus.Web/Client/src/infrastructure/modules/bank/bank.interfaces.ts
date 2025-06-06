export interface GetAllBanksResponse {
    banks : BankResponse[];
}

export type GetBankByIdResponse = BankResponse;

export interface CreateBankRequest {
    name: string;
    description?: string;
}

export interface CreateBankResponse{
    id: string;
}

export interface UpdateBankRequest {
    id: string;
    name: string;
    isActive: boolean;
    description?: string;
}

export interface BankResponse {
    id: string;
    name: string;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}