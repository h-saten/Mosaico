using System.Collections.Generic;

namespace Mosaico.Domain.BusinessManagement
{
    public static class Constants
    {
        public const int MaxCompaniesPerAccount = 15;
        
        public const string Schema = "bsn";
        
        public static class Tables
        {
            public const string Companies = "Companies";
            public const string TeamMember = "TeamMember";
            public const string TeamMemberRole = "TeamMemberRole";
            public const string CompanyInvitation = "CompanyInvitation";
            public const string Verification = "Verification";
            public const string Shareholder = "Shareholder";
            public const string CompanyContactInformation = "CompanyContactInformation";
            public const string CompanySubscribers = "CompanySubscribers";
            public const string Proposals = "Proposals";
            public const string Votes = "Votes";
            public const string Document = "Documents";
        }

        public static class ErrorCodes
        {
            public const string CompanyNotFound = "COMPANY_NOT_FOUND";
            public const string CompanyMustHaveAName = "COMPANY_MUST_HAVE_A_NAME";
            public const string CompanyMustMeetStructureConventions = "COMPANY_MUST_MEET_STRUCTURE_CONVENTIONS";
            public const string TeamMemberNotFound = "TEAM_MEMBER_NOT_FOUND";
            public const string CompanyAlreadyExists = "COMPANY_ALREADY_EXISTS";
            public const string UserNotAuthorizedToAcceptInvitation = "USER_NOT_AUTHORIZED_TO_ACCEPT_INVITATION";
            public const string InvitationAlreadyAccepted = "INVITATION_ALREADY_ACCEPTED";
            public const string InvitationAlreadyExpired = "INVITATION_ALREADY_EXPIRED";
            public const string InvitationAlreadyExists = "INVITATION_ALREADY_EXISTS";
            public const string InvitationNotFound = "INVITATION_NOT_FOUND";
            public const string ShareholdersNotFound = "SHAREHOLDERS_NOT_FOUND";
            public const string CompanyRoleNotFound = "COMPANY_ROLE_NOT_FOUND";
            public const string NotAllowedToRemoveTheOnlyOwner = "NOT_ALLOWED_TO_REMOVE_THE_ONLY_OWNER";
            public const string VerificationNotFound = "VERIFICATION_NOT_FOUND";
            public const string AdminOnly = "ADMIN_ONLY";
            public const string SubscriptionNotFound = "SUBSCRIPTION_NOT_FOUND";
            public const string SubscriptionAlreadyExists = "SUBSCRIPTION_ALREADY_EXISTS";
            public const string ProposalNotFound = "PROPOSAL_NOT_FOUND";
            public const string UnauthorizedProposal = "UNAUTHORIZED_PROPOSAL";
            public const string UnauthorizedVote = "UNAUTHORIZED_VOTE";
            public const string AlreadyVoted = "ALREADY_VOTED";
        }

        public static class TeamMemberRoles
        {
            public const string Owner = "OWNER";
            public const string Member = "MEMBER";
        }

        public static class Invitation
        {
            public const int AcceptanceThreshold = 1;
        }
        
        public static class CompanySizes
        {
            public const string Small = "S";
            public const string Medium = "M";
            public const string Large = "L";
        }
    }
}