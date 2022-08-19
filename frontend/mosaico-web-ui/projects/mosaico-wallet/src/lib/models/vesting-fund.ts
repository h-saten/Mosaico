export interface VestingFund {
    id: string;
    name: string;
    tokenAmount: number;
    startAt: number;
    vestingId: string;
    status: string;
}