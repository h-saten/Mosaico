import { BlockchainNetworkType } from 'mosaico-base';
import { TeamMember, CompanyContactList } from '.';

export interface Company {
    id: string;
    companyName: string;
    companyDescription: string;
    country: string;
    street: string;
    postalCode: string;
    vatId: string;
    teamMembers:	TeamMember[] | null;
    size: number;
    companyContactList: CompanyContactList | null;
    isApproved: boolean;
    logoUrl: string;
    email: string;
    phoneNumber: string;
    region: string;
    network: BlockchainNetworkType;
    isEverybodyCanVoteEnabled: boolean;
    isVotingEnabled: boolean;
    slug: string;
}
