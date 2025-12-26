export default interface BankResponse {
    id: string;
    name: string;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}