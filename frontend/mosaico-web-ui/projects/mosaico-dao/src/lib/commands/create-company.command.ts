import { BlockchainNetworkType } from "mosaico-base";

export interface CreateCompanyCommand {
  companyName: string | null;
  country: string | null;
  street: string | null;
  postalCode: string | null;
  VATId: string | null;
  size: string | null;
  region: string;
  network: BlockchainNetworkType;
  isVotingEnabled: boolean;
  onlyOwnerProposals: boolean;
  quorum: number;
  initialVotingDelay: string;
  initialVotingPeriod: string;
}