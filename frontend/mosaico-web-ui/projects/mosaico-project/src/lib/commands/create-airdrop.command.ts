export interface CreateAirdropCommand {
    name: string;
    startDate: string;
    endDate: string;
    totalCap: number;
    tokensPerParticipant: number;
    stageId: string;
    countAsPurchase: boolean;
}