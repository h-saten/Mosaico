
export interface ImportTokenDetailsResponse {
  name: string;
  symbol: string;
  decimals: number;
  totalSupply: number;
  canImport: boolean;
  burnable: boolean;
  mintable: boolean;
}
