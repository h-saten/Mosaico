using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Mosaico.Base.Exceptions;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.CQRS.Base.Pipelines
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators = null)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators != null && _validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task
                    .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors);
                if (failures.Any(f => f.ErrorCode == Mosaico.Base.Constants.ExceptionCodes.Forbidden))
                {
                    throw new ForbiddenException();
                }

                var failure = failures.FirstOrDefault(f => f != null);
                if (failure != null)
                    throw new ValidationException(failure.ErrorCode, new {message = failure.ErrorMessage});
            }

            return await next();
        }
    }
}