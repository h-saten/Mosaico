import { Company } from '../models';

export interface GetCompanyResponse {
    company: Company;
    isSubscribed: boolean;
}