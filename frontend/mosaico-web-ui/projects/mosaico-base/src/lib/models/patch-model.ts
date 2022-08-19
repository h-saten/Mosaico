export interface PatchModel {
    path: string;
    op: string;
    value: string;
}
export interface PatchModelSocialLinks {
    path: string;
    op: string;
    value: ValueSocialMedia[];
}
export interface ValueSocialMedia {
    key: string;
    value: string;
    isHidden: boolean;
    order: number;
}
