using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSubscribers
{
    public class GetProjectSubscribersQueryHandler : IRequestHandler<GetProjectSubscribersQuery, GetProjectSubscribersQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IMapper _mapper;

        public GetProjectSubscribersQueryHandler(IProjectDbContext projectDbContext, IMapper mapper, IUserManagementClient userManagementClient)
        {
            _projectDbContext = projectDbContext;
            _mapper = mapper;
            _userManagementClient = userManagementClient;
        }

        public async Task<GetProjectSubscribersQueryResponse> Handle(GetProjectSubscribersQuery request, CancellationToken cancellationToken)
        {
            var projectQuery = _projectDbContext.ProjectNewsletterSubscriptions
                .Where(s => s.ProjectId == request.ProjectId).OrderByDescending(t => t.CreatedAt);
            var items = await projectQuery.AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            var count = await projectQuery.CountAsync(cancellationToken);
            var dtos = new List<ProjectSubscriberDTO>();
            var userIds = items.Where(t => !string.IsNullOrWhiteSpace(t.UserId)).Select(t => t.UserId).ToList();
            if (!userIds.Any())
            {
                return new GetProjectSubscribersQueryResponse
                {
                    Entities = new List<ProjectSubscriberDTO>(),
                    Total = 0
                };
            }

            var users = await _userManagementClient.GetUsersAsync(userIds, cancellationToken);
            foreach (var item in items)
            {
                var dto = _mapper.Map<ProjectSubscriberDTO>(item);
                if(!string.IsNullOrWhiteSpace(item.UserId))
                {
                    var user = users.FirstOrDefault(t => t.Id == item.UserId);
                    if (user != null)
                    {
                        dto.Name = $"{user.FirstName} {user.LastName}".Trim();
                    }
                }
                dtos.Add(dto);
            }
            return new GetProjectSubscribersQueryResponse
            {
                Entities = dtos,
                Total = count
            };
        }
    }
}