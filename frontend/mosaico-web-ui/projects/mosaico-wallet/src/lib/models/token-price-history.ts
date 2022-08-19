export interface TokenHistoryRecord {
    date: string;
    rate: number;
}

export interface TokenPriceHistory {
    tokenId?: string;
    tokenName: string;
    tokenSymbol: string;
    currency: string;
    amount: number;
    logo: string;
    totalValue: number;
    latestPrice: number;
    isStakingEnabled: boolean;
    records: TokenHistoryRecord[];
}