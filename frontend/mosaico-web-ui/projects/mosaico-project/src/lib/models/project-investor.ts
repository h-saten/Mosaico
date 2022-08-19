import { ProjectInvestorBalance, ProjectInvestorUserType } from '.';

export interface ProjectInvestor {
    id: string;
    user: ProjectInvestorUserType,
    address: string;
    totalInvestment: number;
    balances: ProjectInvestorBalance[];
}