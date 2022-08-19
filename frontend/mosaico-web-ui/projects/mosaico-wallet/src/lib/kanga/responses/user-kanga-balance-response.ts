import {KangaAsset} from "../models";

export interface UserKangaBalanceResponse {
    assets?: KangaAsset[];
    currency: string;
    totalWalletValue: number;
}
