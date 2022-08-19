using Mosaico.Domain.BusinessManagement.Entities;
using System;
using System.Collections.Generic;

namespace Mosaico.Events.BusinessManagement
{
    public record CompanyCreatedEvent(Guid Id, string CreatedById);
    public record CompanyUpdatedEvent(Guid Id);

    public record CompanyInvitationAddedEvent(Guid CompanyId, string Email);
    public record CompanyLogoUploaded(Guid CompanyId, string LogoUrl);

    public record CompanyInvitationDeletedEvent(Guid CompanyId, string Email, string UserId);
    public record CompanyInvitationUpdatedEvent(Guid CompanyId, Guid InvitationId);
    public record CompanyVerificationCreatedEvent(string Title, Guid CompanyId, Guid VerificationId, List<string> Emails);

    public record SubscribedToCompany(Guid SubscriptionId);

    public record UnsubscribedFromCompany(string Email);
    public record CompanySocialMediaUpdatedEvent(Guid Id);
    public record ProposalVoted(Guid Id, string UserId);

    public record ProposalCreatedEvent(Guid Id, string UserId);

    public record CompanyDocumentUploaded(Guid ProjectDocumentId, string Url);
}