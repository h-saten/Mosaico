import { PaginatedList, PaginationResponse } from 'mosaico-base';
import { ProjectTransactions } from 'mosaico-project';

export interface ProjectTransactionsResponse extends PaginationResponse<ProjectTransactions>  {
}