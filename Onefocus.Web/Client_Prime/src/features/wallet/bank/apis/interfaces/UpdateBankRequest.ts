export default interface UpdateBankRequest {
    id: string;
    name: string;
    isActive: boolean;
    description?: string;
}