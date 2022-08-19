import { KangaPaymentCurrency } from "../../kanga";

export interface InitiateKangaTransactionCommand {
    paymentCurrency: KangaPaymentCurrency; // TODO probably to refactor toward separate type file
    tokenAmount?: number;
    currencyAmount?: number;
    projectId?: string;
    refCode?: string;
}
