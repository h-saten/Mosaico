export interface TeamMember {
    id?: string | null;
    name: string;
    position: string;
    facebook?: string;
    twitter?: string;
    linkedIn?: string;
    order: number;
    profileUrl?: string;
    ProfileFile?: File | null;
    pageId: string;
    photoUrl:string;
}
