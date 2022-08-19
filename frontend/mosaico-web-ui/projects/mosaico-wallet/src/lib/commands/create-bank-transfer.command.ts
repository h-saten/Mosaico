export interface CreateBankTransferCommand {
    currency: string;
    tokenAmount: number;
    fiatAmount: number;
    refCode?: string;
}