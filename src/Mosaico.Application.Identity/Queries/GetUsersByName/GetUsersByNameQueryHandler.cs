using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUsersByName
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersByNameQuery, GetUsersByNameQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        
        public GetUsersQueryHandler(IMapper mapper, IUserReadRepository readRepository, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<GetUsersByNameQueryResponse> Handle(GetUsersByNameQuery request, CancellationToken cancellationToken)
        {
            var usersList = await _readRepository.GetUsersAsync(request.UserName, cancellationToken);
            var users = usersList.Entities.Select(x => _mapper.Map<GetUserQueryResponse>(x)).ToList();
            var result = new PaginatedResult<GetUserQueryResponse>() { Entities = users, Total = usersList.Total };
            var allUsers = new GetUsersByNameQueryResponse
            {
                Users = result
            };

            return allUsers;
        }
    }
}