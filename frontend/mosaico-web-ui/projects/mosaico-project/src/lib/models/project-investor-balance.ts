export interface ProjectInvestorBalance {
    name: string;
    symbol: string;
    id: string;
    balance: number;
    contractAddress: string;
    logoUrl: string;
    isExchangable: boolean;
    isStakable: boolean;
    totalAssetValue: number;
    currency: string;
    isPaymentCurrency: boolean;
    network: string;
}