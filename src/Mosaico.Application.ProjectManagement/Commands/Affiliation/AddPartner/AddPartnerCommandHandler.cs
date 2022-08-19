using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.Exceptions.Affiliation;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.AddPartner
{
    public class AddPartnerCommandHandler : IRequestHandler<AddPartnerCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IUserAffiliationService _affiliationService;
        private readonly IUserManagementClient _managementClient;
    
        public AddPartnerCommandHandler(IProjectDbContext projectDbContext, IUserManagementClient managementClient, IUserAffiliationService affiliationService)
        {
            _projectDbContext = projectDbContext;
            _managementClient = managementClient;
            _affiliationService = affiliationService;
        }
    
        public async Task<Guid> Handle(AddPartnerCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _managementClient.GetUserByEmailAsync(email, cancellationToken);
            if (user == null) throw new UserNotFoundException(request.Email);
            var affiliation =
                await _projectDbContext.ProjectAffiliations.FirstOrDefaultAsync(a => a.ProjectId == project.Id, cancellationToken);
            if (affiliation == null || !affiliation.IsEnabled) throw new AffiliationIsNotActiveException(project.Id);
            var userAffiliation = await _affiliationService.GetOrCreateUserAffiliation(user.Id);
            var partner = userAffiliation.PartnerAssignments.FirstOrDefault(p => p.ProjectAffiliationId == affiliation.Id);
            if (partner != null)
            {
                throw new PartnerAlreadyExistsException(partner.Id);
            }

            partner = new Partner
            {
                Status = PartnerStatus.ENABLED,
                AccessCode = userAffiliation.AccessCode,
                PaymentStatus = PartnerPaymentStatus.PENDING,
                RewardPercentage = affiliation.RewardPercentage,
                UserAffiliation = userAffiliation,
                UserAffiliationId = userAffiliation.Id,
                ProjectAffiliationId = affiliation.Id,
                ProjectAffiliation = affiliation
            };
            _projectDbContext.Partners.Add(partner);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return partner.Id;
        }
    }
}