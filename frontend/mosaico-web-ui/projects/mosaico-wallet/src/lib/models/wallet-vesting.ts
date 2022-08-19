import { TokenBalance } from './token-balance';

export interface VestingWallet {
    id: string;
    claimed: number;
    locked: number;
    nextUnlock?: string;
    totalPeriod?: number;
    startsAt?: string;
    canClaim: boolean;
    tokensToClaim?: number;
    token: TokenBalance;
}