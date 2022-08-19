using System;
using System.Collections.Generic;

namespace Mosaico.Events.ProjectManagement
{
    public record ProjectAcceptedEvent(Guid Id);
    public record ProjectCreatedEvent(Guid Id, string CreatedById);
    public record ProjectVisibilityEvent(Guid Id);
    public record ProjectUpdatedEvent(Guid Id);
    public record StageDeployedEvent(Guid Id);
    public record StageFinalizedEvent(Guid Id);
    public record ProjectMemberAddedEvent(Guid ProjectId, string Email);

    public record ProjectMemberDeletedEvent(Guid ProjectId, string Email, string UserId);
    public record ProjectArticleDeletedEvent(Guid ArticleId);

    public record ProjectArticleHiddenEvent(Guid ArticleId);

    public record ProjectInvitationAcceptedEvent(Guid ProjectId, Guid MemberId);

    public record ProjectMemberUpdatedEvent(Guid ProjectId, Guid MemberId);

    public record ProjectLogoUploaded(Guid ProjectId, string LogoUrl);

    public record PageTeamMemberAdded(Guid PageId, Guid TeamMemberId);
    public record PageTeamMemberUpdated(Guid PageId, Guid TeamMemberId);

    public record PageTeamMemberDeleted(Guid PageId, string MemberName);

    public record PagePartnerAdded(Guid PageId, Guid PartnerId);
    public record PagePartnerUpdated(Guid PageId, Guid PartnerId);

    public record PagePartnerDeleted(Guid PageId, string PartnerName);

    public record ProjectDocumentConverted(Guid DocumentId, string Url);
    public record ProjectDocumentUpdated(Guid ProjectDocumentId);
    public record ProjectDocumentUploaded(Guid ProjectDocumentId, string Url);

    public record PageCoverUploaded(Guid PageId, string Language, string Url);
    public record ArticleCoverUploaded(Guid ArticleId, string Url);
    public record ArticlePhotoUploaded(Guid ArticleId, string Url);

    public record ProjectArticleCreatedEvent(Guid ProjectId, List<string> Emails);
    public record ProjectSubmittedEvent(Guid ProjectId);

    public record ProjectArticleUpdated(Guid ArticleId, Guid CoverId, Guid PhotoId);
    public record InvestorCertificateBackgroundUploaded(Guid ProjectId, string Language, string Url);
    public record InvestorCertificateConfigurationChanged(Guid ProjectId);

    public record DistributeAirdropEvent(Guid AirdropId, string UserId);
    public record InvestmentPackageLogoUpdated(Guid PageId, Guid PackageId, string Url);

    public record RewardPartnerEvent(Guid PartnerId, Guid ProjectId, string UserId);

}