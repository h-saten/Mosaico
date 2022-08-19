import { ProjectPackage } from "./project-packages";
import { Script } from "./script";

export interface Page {
    id: string;
    shortDescription: string;
    coverUrl: string;
    primaryColor: string;
    secondaryColor: string;
    coverColor: string;
    socialMediaLinks: SocialMediaLinks[];
    language: string;
    investmentPackages: ProjectPackage[];
    hasNFTs: boolean;
    hasArticles: boolean;
    hasFAQs: boolean;
    hasInvestmentPackages: boolean;
    scripts: Script[];
    hasReviews: boolean;
}

export interface SocialMediaLinks {
  key: string;
  value: string;
  isHidden: boolean;
  order: number;
}
