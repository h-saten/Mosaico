import { TokenBalance } from './token-balance';

export interface PackagesWallet {
    projectId: string;
    packageId: string;
    startAt?: string;
    endAt?: string;
    days: number;
    token: TokenBalance;
}