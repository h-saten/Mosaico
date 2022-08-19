import {PaymentCurrency, Stage} from '../models';

export interface GetProjectSaleDetailsResponse {
    stage: Stage;
    paymentCurrencies: PaymentCurrency[];
    soldTokens: number;
    companyWalletAddress: string;
    companyWalletNetwork: string;
}
