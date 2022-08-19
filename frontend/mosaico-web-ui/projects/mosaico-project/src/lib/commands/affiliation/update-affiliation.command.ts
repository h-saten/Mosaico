export interface UpdateAffiliationCommand {
    isEnabled: boolean;
    rewardPercentage: number;
    rewardPool: number;
    includeAll: boolean;
    everybodyCanParticipate: boolean;
    partnerShouldBeInvestor: boolean;
}