export default interface UpdateCounterpartyRequest {
    id: string;
    fullName: string;
    email?: string;
    phoneNumber?: string;
    isActive: boolean;
    description?: string;
}