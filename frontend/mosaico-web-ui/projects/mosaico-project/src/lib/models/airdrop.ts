export interface Airdrop {
    name: string;
    slug: string;
    endDate: string;
    isOpened: boolean;
    startDate: string;
    isFinished: boolean;
    totalCap: number;
    tokensPerParticipant: number;
    claimedTokens: number;
    claimedPercentage: number;
    pendingParticipants: number;
    tokenSymbol: string;
    tokenId: string;
    id: string;
    countAsPurchase: boolean;
};