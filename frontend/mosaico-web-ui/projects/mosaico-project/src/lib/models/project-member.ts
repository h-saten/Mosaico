import { ProjectRole } from ".";

export interface ProjectMember {
    id: string;
    isAccepted: boolean;
    email: string;
    userId?: string;
    role: ProjectRole;
};
