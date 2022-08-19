import { ProjectInvestor } from "../models/project-investor";
import { Transaction } from '../models/transaction';

export interface ProjectInvestorQueryResponse {
    user: ProjectInvestor;
    totalInvestment: number;
    totalPayedInUSD: number;
    transactions: Transaction[];
};
