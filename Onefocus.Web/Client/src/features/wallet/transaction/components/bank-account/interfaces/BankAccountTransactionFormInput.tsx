export default interface BankAccountTransactionFormInput {
    id?: string;
    transactedOn: Date;
    currencyId: string;
    amount: number;
    description?: string;
    isActive: boolean;
}