export interface AirdropParticipant {
    email: string;
    walletAddress: string;
    claimed: boolean;
    claimedAt?: string;
    claimedTokenAmount: number;
    withdrawnAt?: string;
    transactionHash: string;
}