export interface UserInformation {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    isAMLVerified: boolean;
    isEmailVerified: boolean;
    username: string;
    photoUrl: string;
    phoneNumber: string;
    isPhoneVerified: boolean;
    isDeactivated: boolean;
    lastLogin: string;
    isAdmin: boolean;
    country: string;
    timezone: string;
    postalCode: string;
    city: string;
    street: string;
    dob: Date;
    hasKangaAccount: boolean;
    evaluationCompleted: boolean;
}
