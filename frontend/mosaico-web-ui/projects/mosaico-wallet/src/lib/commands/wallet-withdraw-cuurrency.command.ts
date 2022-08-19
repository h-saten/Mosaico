export interface WithdrawWalletCurrencyCommand {
    paymentCurrencyId: string;
    address: string;
    amount: number;
}