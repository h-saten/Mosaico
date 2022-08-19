using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetArticles
{
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, GetArticlesQueryResponse>
    {
        private readonly IProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;

        public GetArticlesQueryHandler(IProjectDbContext context, IMapper mapper, ICurrentUserContext currentUserContext)
        {
            _context = context;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<GetArticlesQueryResponse> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            var isAdmin = _currentUserContext.IsGlobalAdmin;

            var articlesQuery = _context.Articles.Where(x => x.ProjectId == request.ProjectId)
                .OrderByDescending(a => a.Date)
                .Where(t => !t.Hidden || isAdmin);

            var articles = await articlesQuery.AsNoTracking()
                .Skip(request.Skip).Take(request.Take)
                .ToListAsync(cancellationToken: cancellationToken);
            
            var totalCount = await articlesQuery.CountAsync(cancellationToken: cancellationToken);
            var dtos = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                var dto = _mapper.Map<ArticleDTO>(article);
                dtos.Add(dto);
            }

            return new GetArticlesQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }



    }
}