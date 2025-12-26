export default interface TransactionItemResponse {
    id?: string;
    name: string;
    amount: number;
    isActive: boolean;
    description?: string;
}