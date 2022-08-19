using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Base;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Identity.Entities
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public bool IsAdmin { get; set; }
        public string DataEventRecordsRole { get; set; }
        public string SecuredFilesRole { get; set; }
        public bool NewsletterDataProcessingAgree { get; set; }
        public DateTime? NewsletterDataProcessingAgreedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime Registered { get; set; }
        public bool MarkedForDeletion { get; set; }
        public bool EvaluationCompleted { get; set; }
        public ApplicationUser()
        {
            NewsletterDataProcessingAgree = false;
            Registered = DateTime.UtcNow;
            AMLStatus = AMLStatus.Unknown;
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.UtcNow;
        }

        public void JoinNewsletter()
        {
            NewsletterDataProcessingAgree = true;
            NewsletterDataProcessingAgreedDate = DateTime.UtcNow;
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        // determines if user has successfully completed KYC
        public AMLStatus AMLStatus { get; set; }
        public DateTimeOffset? AMLVerifiedAt { get; set; }
        public bool IsAMLVerificationDisabled { get; set; }
       
        // Signals that user is deactivated and cannot use the system
        public bool IsDeactivated { get; set; }
        
        // Date and Time when user last time was deactivated
        public DateTimeOffset? DeactivatedAt { get; set; }
        
        // Reason why user was deactivated by admin
        public string DeactivationReason { get; set; }

        //timezone
        public string Timezone { get; set; }
        //country
        public string Country { get; set; }
        //postal code
        public string PostalCode { get; set; }
        //City
        public string City { get; set; }
        //Street
        public string Street { get; set; }
        
        //Date of birth
        public DateTime? Dob { get; set; }

        // Id of the user (admin) who deactivated user
        public Guid? DeactivatedById { get; set; }
        public virtual List<UserToPermission> Permissions { get; set; } = new List<UserToPermission>();
        public Guid? DeletionRequestId { get; set; }
        public virtual DeletionRequest DeletionRequest { get; set; }
        public string Language { get; set; }
        public string PhotoUrl { get; set; }
        // public PhoneNumber PhoneNumber { get; set;}
        public virtual List<PhoneNumberConfirmationCode> PhoneNumberConfirmationCodes { get; set; } = new();
        public virtual KangaUser KangaUser { get; set; }

    }
}