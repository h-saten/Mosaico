export interface CreateVestingCommand {
    tokenId: string;
    name: string;
    numberOfDays: number;
    amountOfClaims: number;
    tokenAmount: number;
    walletAddress: string;
    startsAt: string;
}