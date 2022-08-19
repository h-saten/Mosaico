using System.Collections.Generic;

namespace Mosaico.Domain.ProjectManagement
{
    public static class Constants
    {
        public const string Schema = "prj";
        public const string InvestmentPackageBenefitSeparator = ";";
        public static class Tables
        {
            public const string Projects = "Projects";
            public const string ProjectInvestors = "ProjectInvestors";
            public const string ProjectStatuses = "ProjectStatuses";
            public const string StageStatuses = "StageStatuses";
            public const string Stage = "Stage";
            public const string StageJobs = "StageJobs";
            public const string ProjectMembers = "ProjectMembers";
            public const string ProjectRoles = "ProjectRoles";
            public const string Crowdsales = "Crowdsales";
            public const string Pages = "Pages";
            public const string Faqs = "Faqs";
            public const string PageMembers = "PageMembers";
            public const string PagePartners = "PagePartners";
            public const string FaqTitleTranslations = "FaqTitleTranslations";
            public const string FaqContentTranslation = "FaqContentTranslations";
            public const string PageCoverTranslations = "PageCoverTranslations";
            public const string SocialMediaLinkTranslations = "SocialMediaLinkTranslations";
            public const string ProjectNewsletterSubscriptions = "ProjectNewsletterSubscriptions";
            public const string Documents = "Documents";
            public const string DocumentTemplates = "DocumentTemplates";
            public const string DocumentTypes = "DocumentTypes";
            public const string Abouts = "Abouts";
            public const string AboutContentTranslation = "AboutContentTranslations";
            public const string ShortDescriptionTranslations = "ShortDescriptionTranslations";
            public const string ShortDescription = "ShortDescription";
            public const string InvestmentPackages = "InvestmentPackages";
            public const string InvestmentPackageTranslations = "InvestmentPackageTranslations";
            public const string Articles = "Articles";
            public const string PaymentMethods = "PaymentMethods";
            public const string ProjectPaymentMethods = "ProjectPaymentMethods";
            public const string KangaSale = "KangaSales";
            public const string InvestorCertificate = "InvestorCertificate";
            public const string InvestorCertificateBackground = "InvestorCertificateBackground";
            public const string AirdropCampaigns = "AirdropCampaigns";
            public const string AirdropParticipants = "AirdropParticipants";
            public const string Partners = "Partners";
            public const string ProjectAffiliations = "ProjectAffiliations";
            public const string UserAffiliations = "UserAffiliations";
            public const string StagePurchaseLimits = "StagePurchaseLimits";
            public const string ProjectLikes = "ProjectLikes";
            public const string Scripts = "Scripts";
            public const string PartnerTransactions = "PartnerTransactions";
            public const string UserAffiliationReferences = "UserAffiliationReferences";
            public const string PageReviews = "PageReviews";
        }

        public static class Roles
        {
            public const string Owner = "OWNER";
            public const string Member = "MEMBER";
        }
        
        public static class ErrorCodes
        {
            public const string ProjectNotFound = "PROJECT_NOT_FOUND";
            public const string StageNotFound = "STAGE_NOT_FOUND";
            public const string InvalidProjectStatus = "INVALID_PROJECT_STATUS";
            public const string VestingNotFound = "VESTING_NOT_FOUND";
            public const string VestingAlreadyExists = "VESTING_ALREADY_EXISTS";
            public const string StakingNotFound = "STAKING_NOT_FOUND";
            public const string StakingAlreadyExists = "STAKING_ALREADY_EXISTS";
            public const string FundNotFound = "FUND_NOT_FOUND";
            public const string InvitationNotFound = "INVITATION_NOT_FOUND";
            public const string StageStatusNotFound = "STAGE_STATUS_NOT_FOUND";
            public const string ContractAddressIsEmpty = "CONTRACT_ADDRESS_IS_EMPTY";
            public const string InvalidProject = "INVALID_PROJECT";            
            public const string RoleNotFound = "ROLE_NOT_FOUND";            
            public const string PageNotFound = "PAGE_NOT_FOUND";
            public const string FaqNotFound = "FAQ_NOT_FOUND";
            public const string RoleNotFoundException = "ROLE_NOT_FOUND_EXCEPTION";
            public const string TemplateNotFound = "TEMPLATE_NOT_FOUND_EXCEPTION";
            public const string InvestmentPackageNotFound = "INVESTMENT_PACKAGE_NOT_FOUND";
            public const string DocumentTypeNotFound = "DOCUMENT_TYPE_NOT_FOUND";
            public const string ProjectAlreadyExists = "PROJECT_ALREADY_EXISTS";
        }

        public static class ProjectStatuses
        {
            public const string Approved = "APPROVED";
            public const string New = "NEW";
            public const string UnderReview = "UNDER_REVIEW";
            public const string Closed = "CLOSED";
            public const string InProgress = "IN_PROGRESS";
            public const string Declined = "DECLINED";
        }

        public static class MarketplaceStatuses
        {
            public const string Featured = "FEATURED";
            public const string PublicSale = "PUBLIC_SALE";
            public const string PreSale = "PRE_SALE";
            public const string PrivateSale = "PRIVATE_SALE";
            public const string Upcoming = "UPCOMING";
            public const string SecondaryMarket = "SECONDARY_MARKET";
            public const string Closed = ProjectStatuses.Closed;
        }
        
        public static class DocumentTypes
        {
            public const string Whitepaper = "WHITE_PAPER";
            public const string TermsAndConditions = "TERMS_AND_CONDITIONS";
            public const string PrivacyPolicy = "PRIVACY_POLICY";
        }
        public static class StageStatuses
        {
            public const string Active = "ACTIVE";
            public const string Closed = "CLOSED";
            public const string Pending = "PENDING";
        }

        public static class SocialMediaLinks
        {
            public const string Telegram = "TELEGRAM";
            public const string Facebook = "FACEBOOK";
            public const string Twitter = "TWITTER";
            public const string Private = "PRIVATE";
            public const string YouTube = "YOUTUBE";
            public const string LinkedIn = "LINKEDIN";
            public const string Medium = "MEDIUM";
        }

        public static class PaymentMethods
        {
            public const string KangaExchange = "KANGA_EXCHANGE";
            public const string MosaicoWallet = "MOSAICO_WALLET";
            public const string Binance = "BINANCE";
            public const string Metamask = "METAMASK";
            public const string CreditCard = "CREDIT_CARD";
            public const string BankTransfer = "BANK_TRANSFER";
            public static List<string> All = new() {MosaicoWallet, Metamask, KangaExchange, CreditCard, BankTransfer, Binance };
            public static List<string> ProjectDefault = new() {MosaicoWallet};
        }
    }
}