export default interface CounterpartyResponse {
    id: string;
    fullName: string;
    email?: string;
    phoneNumber?: string;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}