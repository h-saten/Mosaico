import {PaymentMethodType} from "../types";

export interface PaymentMethod {
    id: string;
    disabled: boolean;
    name: string;
    logoUrl: string;
    key: PaymentMethodType;
}
