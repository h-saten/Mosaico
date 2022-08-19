export interface AddProposalCommand {
    title: string;
    tokenId: string;
    network: string;
    description: string;
    quorumThreshold: number;
}
