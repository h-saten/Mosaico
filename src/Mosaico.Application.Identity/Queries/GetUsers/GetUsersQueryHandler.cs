using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Base;
using Mosaico.Base.Extensions;
using Mosaico.Domain.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersQueryResponse>
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

        public async Task<GetUsersQueryResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Email))
            {
                var emailIsBase64 = request.Email.IsBase64String();
                if (emailIsBase64)
                {
                    byte[] email = Convert.FromBase64String(request.Email);
                    string decodedEmail = Encoding.UTF8.GetString(email);
                    request.Email = decodedEmail;
                }
            }

            var usersList = await _readRepository.GetAsync(request.FirstName, request.Email, request.Skip, request.Take, cancellationToken);
            var users = usersList.Entities.Select(x => _mapper.Map<GetUserQueryResponse>(x)).ToList();
            var result = new PaginatedResult<GetUserQueryResponse>() { Entities = users, Total = usersList.Total };
            var allUsers = new GetUsersQueryResponse
            {
                Users = result
            };

            return allUsers;
        }
    }
}