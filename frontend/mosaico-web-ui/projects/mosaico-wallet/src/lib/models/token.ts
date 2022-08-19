import { BlockchainNetworkType } from "mosaico-base";
import { Deflation } from "./deflation";
import { Staking } from "./staking";
import { TokenExternalExchange } from "./token-external-exchange";
import { Vault } from "./vault";
import { Vesting } from "./vesting";

export interface TokenProject {
    id: string;
    slug: string;
    title: string;
    logoUrl: string;
}

export interface Token {
    id: string;
    name: string;
    symbol: string;
    decimals: string;
    address: string;
    network: BlockchainNetworkType;
    isBurnable: boolean;
    isMintable: boolean;
    isVestingEnabled: boolean;
    isDeflationary: boolean;
    logoUrl: string;
    exchanges?: TokenExternalExchange[];
    totalSupply: number;
    type: string;
    status: string;
    companyId: string;
    isGovernance: boolean;
    projects?: TokenProject[];
    deflation?: Deflation;
    vesting?: Vesting;
    stakings?: Staking[];
    vault?: Vault;
    isStakingEnabled?: boolean;
    stakingStartsAt?: string;
}