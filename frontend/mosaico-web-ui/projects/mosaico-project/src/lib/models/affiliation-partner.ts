export interface AffiliationPartner {
    id: string;
    name: string;
    email: string;
    createdAt?: string;
    paymentStatus: string;
    failureReason: string;
    rewardPercentage: number;
    estimatedReward: string;
    transactionsCount: number;
    status: string;
}