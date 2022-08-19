
import { ProjectInvestor } from "../models";

export interface GetProjectInvestorsResponse {
    pageIndex: number;
    totalPages: number;
    totalItems: number;
    items: ProjectInvestor[];
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    id: string;
    projectId: string;
}