export interface OrderValidationResponse {
    status: string;
    message?: string;
    isPhoneNumberRequired: boolean;
}