export default interface GetCounterpartyByIdResponse {
    id: string;
    fullName: string;
    email?: string;
    phoneNumber?: string;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}