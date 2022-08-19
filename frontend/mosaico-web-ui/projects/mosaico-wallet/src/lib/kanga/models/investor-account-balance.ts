export interface InvestorAccountBalance {
    tokenName?: string | null;
    tokenTicker?: string | null;
    currencyAmount: number;
    tokensAmount: number;
    logoUrl?: string | null;
    certificatesEnabled: boolean;
}