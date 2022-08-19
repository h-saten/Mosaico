import { Token } from './token';

export interface PrivateSaleVesting {
    id: string;
    investorCount: number;
    totalSupply: string;
    token: Token;
    claimed: number;
    status: string;
    projectId: string;
    transactionCount: number;
    numberOfDays: number;
    startDate: string;
    endDate: string;
}