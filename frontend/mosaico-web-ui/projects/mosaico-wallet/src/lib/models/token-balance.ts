export interface TokenBalance {
  symbol: string;
  id: string;
  name: string;
  balance: number;
  contractAddress?: string;
  logoUrl: string;
  isExchangable: boolean;
  isStakable: boolean;
  totalAssetValue: number;
  currency: string;
  isPaymentCurrency: boolean;
  network: string;
}
