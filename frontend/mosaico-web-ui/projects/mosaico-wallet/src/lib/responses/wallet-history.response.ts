import { WalletBalanceHistory } from "../models/wallet-balance-history";

export interface GetWalletHistoryResponse {
    balances: WalletBalanceHistory[];
}