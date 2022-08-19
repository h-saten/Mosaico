import { Stage } from '.';
import {PaymentMethodType} from "mosaico-wallet";

export interface Project {
  id: string;
  title: string;
  pageId: string;
  stages: Stage[];
  status: string;
  tokenId?: string;
  companyId?: string;
  activeStage?: Stage;
  hardCap?: number;
  softCap?: number;
  softCapInUserCurrency?: number;
  hardCapInUserCurrency?: number;
  numberOfBuyers?: number;
  raisedCapital?: number;
  raisedCapitalPercentage?: number;
  raisedCapitalSoftCapPercentage?: number;
  isSoftCapAchieved?: boolean;
  logoUrl: string;
  legacyId?: string;
  slug: string;
  isExchangeAvailable: boolean;
  paymentMethods: PaymentMethodType[];
  paymentCurrencies: string[];
  coverLogoUrl: string;
  marketplaceStatus: string;
  raisedCapitalInUSD: number;
  likedByUser?: boolean;
  likeCount: number;
  isFeatured?: boolean;
  isPublic?: boolean;
}
