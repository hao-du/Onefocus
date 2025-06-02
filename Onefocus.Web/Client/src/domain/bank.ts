export interface Bank {
    id: string;
    name: string;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}