export default interface BankAccountTransactionFormInput {
    id?: string;
    transactedOn: Date;
    amount: number;
    description?: string;
    isActive: boolean;
    rowId?: string
}