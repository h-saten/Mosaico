import { TeamMember, CompanyContactList } from "../models";

export interface UpdateCompanyCommand {
    name: string | null;
    country: string | null;
    street: string | null;
    postalCode: string | null;
    VATId: string | null;
    phoneNumber: string | null;
    email: string | null;
    size: number | null;
    companyContacts: CompanyContactList | null;
    teamMembers: TeamMember[] | null;
}