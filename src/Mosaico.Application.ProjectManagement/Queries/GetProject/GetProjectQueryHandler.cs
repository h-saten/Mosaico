using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProject
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, GetProjectQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _context;
        private readonly ICurrentUserContext _currentUser;
        private readonly IProjectDTOAggregatorService _aggregatorService;
        
        public GetProjectQueryHandler(IProjectDbContext context, ICurrentUserContext currentUser, IProjectDTOAggregatorService aggregatorService, ILogger logger = null)
        {
            _context = context;
            _currentUser = currentUser;
            _aggregatorService = aggregatorService;
            _logger = logger;
        }

        public async Task<GetProjectQueryResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.GetProjectOrThrowAsync(request.UniqueIdentifier, cancellationToken);
          
            var isSubscribed = false;
            if (_currentUser.IsAuthenticated)
            {
                isSubscribed = await _context.ProjectNewsletterSubscriptions.AnyAsync(
                    c => c.ProjectId == project.Id && c.UserId == _currentUser.UserId, cancellationToken);
            }

            var dto = await _aggregatorService.FillInDetailDTOAsync(project);
            
            return new GetProjectQueryResponse
            {
                Project = dto,
                IsSubscribed = isSubscribed
            };
        }
    }
}