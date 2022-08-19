export interface UpsertProjectStageCommand {
    name: string;
    type: string;
    startDate: Date;
    endDate: Date;
    tokenSupply: number;
    tokenPrice: number;
    minimumPurchase: number;
    maximumPurchase: number;
    id?: string;
}