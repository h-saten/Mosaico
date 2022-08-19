export interface TransakUserKYC {
    email: string;
    userData: TransakUserData;
}

export interface TransakUserData {
    firstName: string;
    lastName: string;
    email: string;
    mobileNumber: string;
    dob: string;
    address: TransakUserAddress;
}

export interface TransakUserAddress {
    addressLine1?: string;
    addressLine2?: string;
    city?: string;
    state?: string;
    postCode?: string;
    countryCode?: string;
}