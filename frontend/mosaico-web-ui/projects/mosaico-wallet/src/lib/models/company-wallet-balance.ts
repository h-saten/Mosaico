import { CompanyTokenBalance } from ".";

export interface CompanyWalletBalance {
    tokens: CompanyTokenBalance[];
    delta: number;
    deltaDirection: string;
    currency: string;
    address: string;
    totalWalletValue: number;
}