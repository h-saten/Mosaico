import { Nft } from '../models';

export interface GetNFTsResponse {
    total: number;
    entities: Nft[];
} 