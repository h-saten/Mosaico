import { TeamMember } from "./index";

export interface ProductItem {
    id: number;
    title: string;
    icon: string;
    icon_active: string;
    teamLeades?: TeamMember[];
    teamMembers: TeamMember[];
}