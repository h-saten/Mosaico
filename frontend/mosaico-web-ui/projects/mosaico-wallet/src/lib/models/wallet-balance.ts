import { TokenBalance } from '.';

export interface WalletBalance {
    tokens: TokenBalance[];
    delta: number;
    deltaDirection: string;
    currency: string;
    address: string;
    totalWalletValue: number;
};
