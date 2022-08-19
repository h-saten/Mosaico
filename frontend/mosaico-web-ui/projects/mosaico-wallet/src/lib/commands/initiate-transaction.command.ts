import {PaymentProcessor} from "../models";

export interface InitiateTransactionCommand {
    paymentProcessor: PaymentProcessor;
    tokenAmount?: number;
    projectId?: string;
    paymentCurrency: string;
    walletAddress: string;
    network: string;
}
