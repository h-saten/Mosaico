using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUserDeletionRequests
{
    public class GetUserDeletionRequestsQueryHandler : IRequestHandler<GetUserDeletionRequestsQuery, GetUserDeletionRequestsQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        public GetUserDeletionRequestsQueryHandler(IMapper mapper, IUserReadRepository readRepository, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<GetUserDeletionRequestsQueryResponse> Handle(GetUserDeletionRequestsQuery request, CancellationToken cancellationToken)
        {
            var allDeletionRequests = await _readRepository.GetDeletionRequestsAsync(request.Skip, request.Take, cancellationToken);
            var dtos = new List<DeletionRequestDTO>();
            foreach (var req in allDeletionRequests.Entities)
            {
                var dto = _mapper.Map<DeletionRequestDTO>(req);
                dtos.Add(dto);
            }

            return new GetUserDeletionRequestsQueryResponse
            {
                Entities = dtos,
                Total = allDeletionRequests.Total
            };
        }
    }
}