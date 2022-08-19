export interface TokenDistribution {
    id?: string;
    name: string;
    tokenAmount: number;
    tokenPrice?: number;
    projectId?: string;
    balance?: number;
    blocked?: boolean;
};