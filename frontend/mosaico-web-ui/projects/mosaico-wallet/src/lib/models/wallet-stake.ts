import { Token } from './token';

export interface WalletStake {
    id?: string;
    token: Token;
    nextRewardAt?: string;
    balance: number;
    estimatedAPR?: number;
    estimatedRewardInUSD: number;
    days: number;
    walletType?: 'MOSAICO_WALLET' | 'METAMASK';
    version: string;
    wallet: string;
    contractAddress: string;
}