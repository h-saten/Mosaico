using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPageReviews
{
    public class GetTokenPageReviewsQueryHandler : IRequestHandler<GetTokenPageReviewsQuery, List<PageReviewDTO>>
    {
        private readonly IProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTokenPageReviewsQueryHandler(IProjectDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<PageReviewDTO>> Handle(GetTokenPageReviewsQuery request, CancellationToken cancellationToken)
        {
            var pageReviews = await _dbContext.PageReviews.Where(t => t.PageId == request.PageId).ToListAsync(cancellationToken);
            return pageReviews.Select(t => _mapper.Map<PageReviewDTO>(t)).ToList();
        }
    }
}