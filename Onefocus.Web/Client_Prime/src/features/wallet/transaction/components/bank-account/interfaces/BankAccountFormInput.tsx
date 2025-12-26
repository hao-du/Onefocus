import BankAccountTransactionFormInput from "./BankAccountTransactionFormInput";

export default interface BankAccountFormInput {
    id?: string;
    amount: number;
    currencyId: string;
    interestRate: number;
    accountNumber: string;
    issuedOn: Date;
    closedOn?: Date;
    isClosed: boolean;
    bankId: string;
    description?: string;
    isActive: boolean;
    transactions: BankAccountTransactionFormInput[];
}