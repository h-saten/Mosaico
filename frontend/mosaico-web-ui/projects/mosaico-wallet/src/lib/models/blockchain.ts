import { BlockchainNetworkType } from "mosaico-base";

export interface Blockchain {
    name: BlockchainNetworkType;
    endpoint: string;
    logoUrl: string;
    isDefault: boolean;
    chainId: string;
    etherscanUrl: string;
}