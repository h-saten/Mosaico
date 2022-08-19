import {Transaction} from "../models/transaction";
import { PaginationResponse } from 'mosaico-base';

export interface CompanyWalletTransactionsResponse extends PaginationResponse<Transaction> {
}
