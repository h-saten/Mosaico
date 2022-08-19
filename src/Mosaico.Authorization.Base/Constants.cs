using System.Collections.Generic;

namespace Mosaico.Authorization.Base
{
    public static class Constants
    {
        public static class DefaultRoles
        {
            public const string Self = "SELF";
            public const string Admin = "ADMIN";
        }
        
        public static class Permissions
        {
            public static class Token
            {
                public static List<string> GetAll() => new()
                {
                    CanEdit,
                    CanRead,
                };
                public const string CanRead = "CAN_READ";
                public const string CanEdit = "CAN_EDIT";
            }
            
            public static class UserProfile
            {
                public static List<string> GetAll() => new()
                {
                    CanEditPassword,
                    CanRead,
                    CanEditEmail,
                    CanEditProfile,
                    CanEditPhone
                };

                public const string CanEditPassword = "CAN_EDIT_PASSWORD";
                public const string CanRead = "CAN_READ";
                public const string CanEditEmail = "CAN_EDIT_EMAIL";
                public const string CanEditProfile = "CAN_EDIT_PROFILE";
                public const string CanEditPhone = "CAN_EDIT_PHONE";
                public const string CanVerifyAccount = "CAN_VERIFY_ACCOUNT";
            }
            public static class Company
            {
                public static List<string> GetAll() => new()
                {
                    { CanEditDetails },
                    { CanRead },
                    { CanReadCompanyWallet }
                };
                public const string CanEditDetails = "CAN_EDIT_DETAILS";
                public const string CanRead = "CAN_READ";
                public const string CanReadCompanyWallet = "CAN_READ_COMPANY_WALLET";
            }
            
            public static class Project
            {
                public static List<string> GetAll() => new()
                {
                    {CanPurchase},
                    {CanEditDetails},
                    {CanEditStages},
                    {CanEditVesting},
                    {CanEditDocuments},
                    {CanRead},
                    {CanEditStaking}
                };
            
                public const string CanPurchase = "CAN_PURCHASE";
                public const string CanEditDetails = "CAN_EDIT_DETAILS";
                public const string CanEditStages = "CAN_EDIT_STAGES";
                public const string CanEditVesting = "CAN_EDIT_VESTING";
                public const string CanEditStaking = "CAN_EDIT_STAKING";
                public const string CanEditDocuments = "CAN_EDIT_DOCUMENTS";
                public const string CanRead = "CAN_READ";
                public const string CanViewDashboard = "CAN_VIEW_DASHBOARD";

            }
        }
        
        public static class ScopesConstants
        {
            public const string IcoAdmin = "tokenizer.icoadmin";
            public const string GlobalAdmin = "tokenizer.globaladmin";
            public const string PublicData = "tokenizer.publicdata";
            public const string BetaTester = "tokenizer.beta_tester";
        }
        
        public static class ErrorCodes
        {
            public const string AttributeNotSet = "AUTH_ATTRIBUTE_NOT_SET";
            public const string Unauthorized = "USER_UNAUTHORIZED";
        }
    }
}