import { Token } from './token';
import { VestingFund } from './vesting-fund';

export interface PersonalVesting {
    id: string;
    name: string;
    walletAddress: string;
    tokenAmount: string;
    token?: Token;
    claimed?: number;
    status?: string;
    transactionCount?: number;
    numberOfDays: number;
    startsAt: string;
    initialPaymentPercentage: number;
    tokenId: string;
    funds: VestingFund[];
    percentageCompleted?: number;
}