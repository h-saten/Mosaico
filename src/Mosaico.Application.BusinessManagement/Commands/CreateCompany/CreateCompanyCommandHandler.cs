using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Base.Extensions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.SignalR.DTO;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyCreatedDTO>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ICompanyPermissionFactory _permissionFactory;

        public CreateCompanyCommandHandler(IBusinessDbContext dbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, 
            IMapper mapper, ICurrentUserContext currentUserContext,
            ICompanyPermissionFactory permissionFactory, ILogger logger = null)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _currentUserContext = currentUserContext;
            _permissionFactory = permissionFactory;
        }

        public async Task<CompanyCreatedDTO> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {         
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to create a company");
                    
                    var company = _mapper.Map<Company>(request);
                    company.Slug = company.CompanyName.ToLowerInvariant().ToSlug();
                    
                    if (_context.Companies.Any(c => c.Slug == company.Slug))
                    {
                        throw new CompanyAlreadyExistsException(request.CompanyName);
                    }
                    
                    var ownerRole = _context.TeamMemberRoles.FirstOrDefault(s => s.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner);
                    if (ownerRole == null)
                    {
                        throw new CompanyRoleNotFoundException(Domain.BusinessManagement.Constants.TeamMemberRoles.Owner);
                    }

                    var ownerCompaniesCount = await _context.Companies.CountAsync(
                        c => c.CreatedById == _currentUserContext.UserId, cancellationToken: cancellationToken);
                    
                    if (ownerCompaniesCount >= Domain.BusinessManagement.Constants.MaxCompaniesPerAccount)
                    {
                        throw new LimitExceededException(nameof(Company));
                    }
                    
                    var ownerTM = new TeamMember
                    {
                        CompanyId = company.Id,
                        UserId = _currentUserContext.UserId,
                        TeamMemberRole = ownerRole,
                        TeamMemberRoleId = ownerRole.Id,
                        AcceptedAt = DateTimeOffset.UtcNow,
                        ExpiresAt = DateTimeOffset.UtcNow,
                        IsInvitationSent = true,
                        Email = _currentUserContext.Email,
                        IsAccepted = true
                    };

                    company.IsApproved = false;
                    company.TeamMembers.Add(ownerTM);
                    _context.TeamMembers.Add(ownerTM);
                    _logger?.Verbose($"Owner was added to company team members");

                    _context.Companies.Add(company);
                    if (string.IsNullOrEmpty(company.CompanyName))
                    {
                        throw new CompanyMustHaveAName();
                    }
                    if (_context.Companies.Any(x => x.CompanyName == company.CompanyName))
                    {
                        throw new CompanyAlreadyExistsException(company.CompanyName);
                    }
                    var socialMedia = new SocialMediaLinks
                    {
                        CompanyId = company.Id
                    };
                    _context.SocialMediaLinks.Add(socialMedia);
                    await _permissionFactory.AddUserPermissionsAsync(company.Id, _currentUserContext.UserId, await _permissionFactory.GetRolePermissionsAsync(Domain.BusinessManagement.Constants.TeamMemberRoles.Owner));
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Company was successfully added");
                    await PublishCompanyCreatedEventAsync(company);
                    _logger?.Verbose($"Events were sent");
                    await transaction.CommitAsync(cancellationToken);
                    return new CompanyCreatedDTO
                    {
                        Slug = company.Slug,
                        CompanyId = company.Id
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishCompanyCreatedEventAsync(Company company)
        {
            var eventPayload = new CompanyCreatedEvent(company.Id, company.CreatedById);
            var @event = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}