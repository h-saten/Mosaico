import { ExchangeRate, PaymentCurrency, TokenBalance } from "../models";

export interface CCQuoteResponse {
    exchangeRates: ExchangeRate[];
    paymentAddress: string;
    companyName: string;
    projectName: string;
    regulationsUrl: string;
    privacyPolicyUrl: string;
    currencies: PaymentCurrency[];
    minimumPurchase: number;
    maximumPurchase: number;
    paymentCurrencyBalances?: TokenBalance[];
}