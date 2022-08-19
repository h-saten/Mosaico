export interface Invitation {
    id: string;
    isAccepted: boolean;
    expiresAt?: string;
    projectId: string;
    projectTitle: string;
    tokenName: string;
    distributionPerPerson: number;
    days: number;
};