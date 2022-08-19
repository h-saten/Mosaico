export interface TeamMember {
    id?: string | null;
    firstName: string;
    lastName: string;
    companyId: string;
    isAccepted: boolean;
    email: string;
    role: string;
    isExpired?: boolean;
}
