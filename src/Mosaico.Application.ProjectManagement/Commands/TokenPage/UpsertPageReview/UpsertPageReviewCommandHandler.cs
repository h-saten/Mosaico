using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertPageReview
{
    public class UpsertPageReviewCommandHandler : IRequestHandler<UpsertPageReviewCommand, Guid>
    {
        private readonly IProjectDbContext _dbContext;
        private readonly ILogger _logger;

        public UpsertPageReviewCommandHandler(IProjectDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpsertPageReviewCommand request, CancellationToken cancellationToken)
        {
            PageReview review;
            if (request.Id.HasValue)
            {
                review = await _dbContext.PageReviews.FirstOrDefaultAsync(t => t.PageId == request.PageId,
                    cancellationToken: cancellationToken);
                if (review == null)
                {
                    throw new PageReviewNotFoundException(request.Id.Value);
                }
            }
            else
            {
                review = new PageReview
                {
                    PageId = request.PageId
                };
                _dbContext.PageReviews.Add(review);
            }

            review.Category = request.Category;
            review.Link = request.Link;
            review.IsHidden = request.IsHidden;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return review.Id;
        }
    }
}