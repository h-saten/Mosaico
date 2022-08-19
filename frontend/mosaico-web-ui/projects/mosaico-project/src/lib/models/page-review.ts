import { SafeUrl } from "@angular/platform-browser";

export interface PageReview {
    id: string;
    link: string;
    safeLink?: SafeUrl;
    isHidden: boolean;
    category:  'YOUTUBE' | 'FACEBOOK' | 'LINKEDIN';
}