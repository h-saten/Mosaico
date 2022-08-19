using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Commands.CompleteEvaluation
{
    public class CompleteEvaluationCommandHandler : IRequestHandler<CompleteEvaluationCommand>
    {
        private readonly IIdentityContext _identityContext;

        public CompleteEvaluationCommandHandler(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(CompleteEvaluationCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            if (!user.EvaluationCompleted)
            {
                var userDbResponses = await _identityContext.UserEvaluationQuestions.Where(ueq => ueq.UserId == request.UserId).ToListAsync(cancellationToken);
                foreach (var response in request.Responses)
                {
                    var dbResponse = userDbResponses.FirstOrDefault(ueq => ueq.Key == response.Key && ueq.UserId == user.Id);
                    if (dbResponse == null)
                    {
                        _identityContext.UserEvaluationQuestions.Add(new UserEvaluationQuestion
                        {
                            Key = response.Key,
                            Response = response.Response,
                            UserId = user.Id
                        });
                    }
                    else
                    {
                        dbResponse.Response = response.Response;
                    }
                }
                user.EvaluationCompleted = true;
                await _identityContext.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}