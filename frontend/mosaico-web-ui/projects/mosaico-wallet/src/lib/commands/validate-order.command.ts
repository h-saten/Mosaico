export interface ValidateOrderCommand {
    tokenAmount: number;
    currency: string;
    payedAmount: number;
    paymentMethod: string;
}