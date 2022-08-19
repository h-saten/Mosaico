import { PaginationResponse } from "mosaico-base";
import {Transaction} from "../models/transaction";

export interface WalletTransactionsResponse extends PaginationResponse<Transaction> {
}
