import { Shareholder } from "../models/shareholder";

export interface AddVerificationCommand {
    companyRegistrationUrl: string;
    companyAddressUrl: string;
    shareholders: Shareholder[];
    companyId: string | null;
    companyName: string | null;
};
