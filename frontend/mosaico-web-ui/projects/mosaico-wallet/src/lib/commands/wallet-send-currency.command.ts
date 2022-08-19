export interface SendWalletCurrencyCommand {
    paymentCurrencyId: string;
    address: string;
    amount: number;
}