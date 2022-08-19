import { CompanyContactList, CompanyListProject } from '.';

export interface CompanyList {
  id: string;
  companyName: string;
  logoUrl: string;
  isApproved: boolean;
  isSubscribed: boolean;
  slug: string;
  companyDescription: string;
  projects: CompanyListProject[];
  totalProposals: number;
  openProposals: number;
  network: string;
}
