import BankAccountTransactionDto from "./BankAccountTransactionDto";

export default interface CreateBankAccountRequest {
    amount: number;
    currencyId: string;
    interestRate: number;
    accountNumber: string;
    issuedOn: Date;
    closedOn?: Date;
    isClosed: boolean;
    bankId: string;
    description?: string;
    transactions: BankAccountTransactionDto[];
}