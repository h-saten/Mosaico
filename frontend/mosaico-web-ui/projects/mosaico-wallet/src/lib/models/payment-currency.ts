
export interface PaymentCurrency {
    id: string;
    name: string;
    ticker: string;
    logoUrl: string | null;
    contractAddress: string;
    nativeChainCurrency: boolean;
}
