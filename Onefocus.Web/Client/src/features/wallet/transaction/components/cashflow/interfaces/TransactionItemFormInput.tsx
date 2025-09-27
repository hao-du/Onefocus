export default interface TransactionItemFormInput {
    id?: string;
    name: string;
    amount: number;
    isActive: boolean;
    description?: string;
}