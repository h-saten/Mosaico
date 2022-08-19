import { TokenPriceHistory } from "../models/token-price-history";

export interface VentureFundHistoryResponse {
    tokens: TokenPriceHistory[];
    totalAssetValue: number;
    lastUpdatedAt?: string;
}