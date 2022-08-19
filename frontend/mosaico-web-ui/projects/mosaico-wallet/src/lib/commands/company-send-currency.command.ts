export interface SendCompanyCurrencyCommand {
    paymentCurrencyId: string;
    address: string;
    amount: number;
}