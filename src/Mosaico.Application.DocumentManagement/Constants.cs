using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement
{
    public static class Constants
    {
        public static class Containers
        {
            public const string DefaultDocumentContainer = "ci-documents";
            public const string ProjectTeamMemberProfileContainer = "ci-project-team-member-profile";
            public const string ProjectPartnerProfileContainer = "ci-project-partner-profile";
            public const string CompanyDocumentFileContainer = "ci-company-document-file";
            public const string CertificateBackgroundContainer = "ci-project-certificate-background";

        }
        public static class ErrorCodes
        {
            public const string ProjectNotFound = "PROJECT_NOT_FOUND";
            public const string DuplicateDocumentTitle = "DUPLICATE_DOCUMENT_TITLE";
            public const string DocumentLanguageNotAvailable = "DOCUMENT_LANGUAGE_NOT_AVAILABLE";
            public const string DocumentLanguageAlreadyExists = "DOCUMENT_LANGUAGE_ALREADY_EXISTS";
            public const string DocumentIsMandatory = "DOCUMENT_Is_MANDATORY";
        }

        public static class MandatoryProjectDocuments
        {
            public const string Whitepaper = "Whitepaper";
            public const string Regulations = "Regulations";
            public const string PrivacyPolicy = "Privacy Policy";
        }
    }
}
