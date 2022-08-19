import { BlockchainNetworkType } from "mosaico-base";
import { ERC20ContractVersion } from '../services';

export interface CreateTokenCommand {
    name: string;
    symbol: string;
    decimals: number;
    network: BlockchainNetworkType;
    initialSupply: number;
    tokenType: string;
    isMintable: boolean;
    isBurnable: boolean;
    companyId: string;
    projectId?: string;
    ownerAddress?: string;
    contractAddress?: string;
    contractVersion?: ERC20ContractVersion;
    isGovernance: boolean;
}
