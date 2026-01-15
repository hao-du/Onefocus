export default interface BankAccountTransactionDto {
    id?: string;
    transactedOn: Date;
    amount: number;
    description?: string;
    isActive: boolean;
}