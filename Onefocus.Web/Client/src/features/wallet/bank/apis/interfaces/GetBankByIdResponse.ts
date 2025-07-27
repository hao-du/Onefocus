export default interface GetBankByIdResponse {
    id: string;
    name: string;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}