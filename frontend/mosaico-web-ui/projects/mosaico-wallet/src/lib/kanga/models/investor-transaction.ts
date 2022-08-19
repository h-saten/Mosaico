import { KangaPaymentCurrency } from "./kanga-currency";

export interface InvestorTransaction {
    transactionId: string;
    currencyAmount: number;
    tokensAmount: number;
    createdAt: string;
    tokenTicker: string;
    tokenName: string;
    currency: KangaPaymentCurrency;
}