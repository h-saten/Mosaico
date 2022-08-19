import { Shareholder } from '.';

export interface Verification {
    companyRegistrationUrl: string;
    companyAddressUrl: string;
    shareholders: Shareholder[];
    companyId: string | null;
    companyName: string | null;
}
