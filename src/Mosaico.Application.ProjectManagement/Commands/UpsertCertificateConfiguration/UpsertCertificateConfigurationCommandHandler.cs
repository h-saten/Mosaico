using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertCertificateConfiguration
{
    public class UpsertCertificateConfigurationCommandHandler : IRequestHandler<UpsertCertificateConfigurationCommand>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;

        public UpsertCertificateConfigurationCommandHandler(
            ILogger logger, 
            IProjectDbContext projectDbContext, 
            IEventPublisher eventPublisher,
            IEventFactory eventFactory)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
        }

        public async Task<Unit> Handle(UpsertCertificateConfigurationCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create project article");
            
            var project = await _projectDbContext
                .Projects
                .Include(x=> x.InvestorCertificate)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            _logger?.Verbose($"Project {request.ProjectId} was found");

            var certificateConfiguration = new CertificateConfiguration
            {
                Code = request.Code,
                Date = request.Date,
                InvestorName = request.Name,
                LogoBlock = request.Logo,
                TokensAmount = request.Tokens
            };
            
            var investorCertificate = project.InvestorCertificate;
            if (investorCertificate is null)
            {
                investorCertificate = new InvestorCertificate(project.Id, certificateConfiguration);
                project.InvestorCertificate = investorCertificate;
                await _projectDbContext.InvestorCertificates.AddAsync(investorCertificate, cancellationToken);
            }
            else
            {
                investorCertificate.UpdateConfiguration(certificateConfiguration);
            }

            if (request.SendingEnabled)
            {
                investorCertificate.EnableSending();
            }
            else
            {
                investorCertificate.DisableSending();
            }
            
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            
            await PublishEvents(project.Id);
            return Unit.Value;
        }

        private async Task PublishEvents(Guid projectId)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new InvestorCertificateConfigurationChanged(projectId));
            await _eventPublisher.PublishAsync(e);
        }

    }
}