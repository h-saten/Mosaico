import { BlockchainNetworkType } from "mosaico-base";
import { Stage } from "./stage";

export interface ProjectsList {
    id: string;
    title: string;
    shortDescription: string;
    tokenId?: string;
    tokenName: string;
    tokenSymbol: string;
    logoUrl: string;
    status: string;
    network: BlockchainNetworkType;
    activeStage?: Stage;
    softCap: number;
    hardCap: number;
    raisedCapital: number;
    raisedCapitalPercentage: number;
    numberOfBuyers: number;
    pageId: string;
    slug: string;
    isVisible: boolean;
    isExchangeAvailable: boolean;
    coverLogoUrl: string;
    marketplaceStatus: string;
    raisedCapitalInUSD: number;
    raisedCapitalSoftCapPercentage?: number;
    isSoftCapAchieved?: boolean;
    hardCapInUserCurrency?: number;
    softCapInUserCurrency?: number;
    likedByUser?: boolean;
    likeCount: number;
    isFeatured: boolean;
    isBlockedForEditing: boolean;
    isUserSubscribeProject: boolean;
}
