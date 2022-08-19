export interface EnableVestingCommand {
    startsAt?: string;
    tokenDistributionId?: string;
    stageId?: string;
    targetType: string;
    days: number;
    distributionPerPerson?: number;
    canWithdrawEarly?: boolean;
    releaseCycleInDays?: number;
    subtractedPercent?: number;
    email?: string;
    tokenAmount?: number;
}
