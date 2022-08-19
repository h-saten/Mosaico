
export interface PaymentCurrency {
  symbol: string;
  name: string;
  contractAddress: string;
  exchangeRate: number;
  isNativeCurrency: boolean;
}
