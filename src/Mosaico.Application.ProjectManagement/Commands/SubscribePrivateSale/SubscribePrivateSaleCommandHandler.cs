using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.SubscribePrivateSale
{
    public class SubscribePrivateSaleCommandHandler : IRequestHandler<SubscribePrivateSaleCommand, SubscribePrivateSaleCommandResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _userContext;

        public SubscribePrivateSaleCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext userContext)
        {
            _projectDbContext = projectDbContext;
            _userContext = userContext;
        }

        public async Task<SubscribePrivateSaleCommandResponse> Handle(SubscribePrivateSaleCommand request, CancellationToken cancellationToken)
        {
            var stage = await _projectDbContext.Stages.Include(s => s.Status)
                .Include(s => s.Project)
                .Include(p => p.ProjectInvestors)
                .FirstOrDefaultAsync(s =>
                s.AuthorizationCode == request.AuthorizationCode, cancellationToken: cancellationToken);
            
            if (stage == null || stage.Type == StageType.Public || stage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Closed)
            {
                throw new StageNotFoundException(Guid.Empty);
            }

            if (!stage.ProjectInvestors.Any(pi => pi.UserId == _userContext.UserId))
            {
                _projectDbContext.ProjectInvestors.Add(new ProjectInvestor
                {
                    IsAllowed = true,
                    UserId = _userContext.UserId,
                    Stage = stage,
                    StageId = stage.Id
                });
                await _projectDbContext.SaveChangesAsync(cancellationToken);
            }

            return new SubscribePrivateSaleCommandResponse
            {
                Slug = stage.Project.Slug,
                Title = stage.Project.Title
            };
        }
    }
}