export default interface GetAllBanksResponse {
    banks : {
        id: string;
        name: string;
        isActive: boolean;
        description?: string;
        actionedOn: Date;
        actionedBy: string;
    }[];
}