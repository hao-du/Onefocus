export default interface BankAccountTransactionDto {
    id?: string;
    transactedOn: Date;
    currencyId: string;
    amount: number;
    description?: string;
    isActive: boolean;
}