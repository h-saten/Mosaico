export interface UpdateBankDetailsCommand {
    projectId: string;
    account: string;
    bankName: string;
    swift: string;
    key: string;
    accountAddress: string;
}