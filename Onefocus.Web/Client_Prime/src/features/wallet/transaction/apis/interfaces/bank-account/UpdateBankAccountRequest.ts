import BankAccountTransactionDto from "./BankAccountTransactionDto";

export default interface UpdateBankAccountRequest {
    id: string;
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
    transactions: BankAccountTransactionDto[];
}