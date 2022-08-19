import { VotingStatus } from "./voting-status";

export interface Proposal {
    id: string;
    title: string;
    startsAt: string;
    endsAt: string;
    createdByAddress: string;
    status: VotingStatus;
    proposalId: string;
    quorumThreshold: number;
    network: string;
    tokenId: string;
    voteCount: number;
    forCount: number;
    againstCount: number;
    abstainCount: number;
    quorumReached: number;
    description: string;
    forCountPercentage: number;
    againstCountPercentage: number;
    abstainCountPercentage: number;
}
