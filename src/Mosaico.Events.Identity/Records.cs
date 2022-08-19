using System;
using System.Collections.Generic;

namespace Mosaico.Events.Identity
{
    public record UserCreatedEvent(string Id);
    public record ExternalUserCreatedEvent(string Id);
    public record UserDeleteRequestedEvent(string Id);
    public record UserDeletedEvent(string Id);
    public record UserRestoredEvent(string Id);
    public record UserStolenAccountEvent(string Id);
    public record UserLoggedInEvent(string Id);
    public record UserVerifiedEvent(string Id);
    public record UserEmailConfirmedEvent(string Id);
    public record UserDeactivated(string Id);
    public record UserActivated(string Id);
    public record UserInitiatedPasswordReset(string Id);
    public record UserInitiatedPasswordChange(string Id, string code);

    public record UserConfirmedPasswordChange(string Id, string email, string code);
    public record UserUpdatedEvent(string Id);

    public record UserEmailChanged(string Id, string OldEmail, string code);
    public record UserInitiatedEmailChange(string Id, string Email);
    public record RemoveUserPermissionsRequested(string Id, Dictionary<string, Guid?> PermissionsToDelete);
    public record AddUserPermissionsRequested(string Id, Dictionary<string, Guid?> PermissionsToAdd);
    public record UserPhotoUploaded(string UserId, string PhotoUrl);
    public record UserPhoneNumberVerified(string UserId, string Code = null);

    public record UserCountUpdated(long Count);
}