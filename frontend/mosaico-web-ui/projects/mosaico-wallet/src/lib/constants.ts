import { BlockchainNetworkType } from "mosaico-base";
import { TokenType } from "./models";

export interface NetworkOption {
    name: BlockchainNetworkType;
    logoUrl: string;
    isEnabled: boolean;
}

export interface TokenTypeOption {
    key: TokenType;
    name: string;
    disabled: boolean;
    description: string;
}

export const TOKEN_TYPES: TokenTypeOption[] = [
    {
        key: "UTILITY",
        name: 'NEW_TOKEN.TOKEN_TYPES.UTILITY',
        disabled: false,
        description: 'NEW_TOKEN.TOKEN_TYPES.UTILITY_HINT'
    },
    {
        key: 'SECURITY',
        name: 'NEW_TOKEN.TOKEN_TYPES.SECURITY',
        disabled: true,
        description: 'NEW_TOKEN.TOKEN_TYPES.SECURITY_HINT'
    },
    {
        key: 'GOVERNANCE',
        name: 'NEW_TOKEN.TOKEN_TYPES.GOVERNANCE',
        disabled: false,
        description: 'NEW_TOKEN.TOKEN_TYPES.GOVERNANCE_HINT'
    }
];

export enum TOKEN_PERMISSIONS {
    "CAN_EDIT" = "CAN_EDIT",
    "CAN_READ" = "CAN_READ"
};