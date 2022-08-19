export interface Operation 
{
    id: string;
    transactionId: string;
    type: string;
    state: string;
    finishedAt: string;
    retryAttempts: number;
    transactionHash: string;
    userId: string;
    payedNativeCurrency: number;
    payedInUSD: number;
    network: string;
    accountAddress: string;
    contractAddress: string;
    projectId?: string;
    createdAt: string;
}