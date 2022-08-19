export interface DeploymentEstimate {
    gas: number;
    contractVersion: string;
    gasPrice: number;
    price: number;
    network: string;
    currency: string;
    estimatedAt: string;
    paymentMethod: string;
    fee: number;
    nativeCurrencyTicker: string;
    nativeCurrencyAmount: number;
}