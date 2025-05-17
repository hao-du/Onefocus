export interface Currency {
    id: string;
    name: string;
    shortName: string;
    defaultFlag: boolean;
    activeFlag: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}