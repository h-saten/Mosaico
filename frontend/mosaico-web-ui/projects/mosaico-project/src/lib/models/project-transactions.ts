export interface ProjectTransactions {
    id: string;
    userName: string;
    projectName: string;
    tranHash: string;
    purchasedDate: Date;
    source: string;
    payedAmount: number;
    tokenAmount: number;
    userWallet: string;
    paymentCurrencySymbol: string;
    currency: string;
    transactionId: string;
    status: string;
    payedInUSD: number;
    tokenSymbol: string;
    failureReason?: string;
    paymentMethod?: string;
    tokenPrice?: number;
    gasFee?: number;
    feeInUSD?: number;
    mosaicoFee?: number;
    mosaicoFeeInUSD?: number;
    feePercentage?: number;
    exchangeRate?: number;
    salesAgentId?: string;
    fee?: number;
    externalLink?: string;
}