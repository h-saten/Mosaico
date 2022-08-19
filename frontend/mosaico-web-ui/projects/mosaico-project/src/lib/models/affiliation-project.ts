import { Token } from 'mosaico-wallet';

export interface AffiliationProject {
    projectId: string;
    projectTitle: string;
    projectSlug: string;
    transactionsCount: number;
    estimatedReward: number;
    token: Token;
}