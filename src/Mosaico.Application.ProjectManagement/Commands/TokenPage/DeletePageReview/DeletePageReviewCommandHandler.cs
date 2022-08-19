using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeletePageReview
{
    public class DeletePageReviewCommandHandler : IRequestHandler<DeletePageReviewCommand>
    {
        private readonly IProjectDbContext _dbContext;

        public DeletePageReviewCommandHandler(IProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeletePageReviewCommand request, CancellationToken cancellationToken)
        {
            var review =
                await _dbContext.PageReviews.FirstOrDefaultAsync(t => t.Id == request.Id && t.PageId == request.PageId,
                    cancellationToken);
            if (review == null)
            {
                throw new PageReviewNotFoundException(request.Id);
            }

            _dbContext.PageReviews.Remove(review);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}