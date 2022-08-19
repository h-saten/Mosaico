import { AffiliationProject } from "../models";

export interface GetUserAffiliationResponse {
    id: string;
    accessCode: string;
    projects: AffiliationProject[];
}