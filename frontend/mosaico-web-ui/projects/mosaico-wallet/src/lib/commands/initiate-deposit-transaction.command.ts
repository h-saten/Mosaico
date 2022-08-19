import {PaymentProcessor} from "../models";

export interface InitiateDepositTransactionCommand {
    paymentProcessor: PaymentProcessor;
}
