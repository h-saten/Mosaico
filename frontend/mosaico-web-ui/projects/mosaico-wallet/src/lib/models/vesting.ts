import { VestingFund } from "./vesting-fund";

export interface Vesting {
    id: string;
    tokenId: string;
    projectId: string;
    startsAt: string;
    funds: VestingFund[];
}