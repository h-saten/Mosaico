export interface Stage {
    id?: string;
    name: string;
    type: string;
    startDate: Date;
    endDate: Date;
    tokensSupply: number;
    tokenPrice: number;
    minimumPurchase: number;
    maximumPurchase: number;
    status?: string;
    softcap: number;
    hardcap: number;
    projectId?: string;
    order?: number;
};
