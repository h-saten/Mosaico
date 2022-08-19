using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertPaymentMethod
{
    public class UpsertPaymentMethodCommandHandler : IRequestHandler<UpsertPaymentMethodCommand>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICacheClient _cacheClient;

        public UpsertPaymentMethodCommandHandler(ILogger logger, IProjectDbContext projectDbContext, ICacheClient cacheClient)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _cacheClient = cacheClient;
        }

        public async Task<Unit> Handle(UpsertPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create project document");
            
            var project = await _projectDbContext
                .Projects
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            _logger?.Verbose($"Project {request.ProjectId} was found");
            
            var paymentMethods = project.PaymentMethods;

            var method = paymentMethods.FirstOrDefault(x => x.Key == request.PaymentMethodKey);
            
            if (request.IsEnabled)
            {
                if (method is not null) return Unit.Value;
                var paymentMethod = await _projectDbContext
                    .PaymentMethods
                    .Where(x => x.Key == request.PaymentMethodKey)
                    .FirstOrDefaultAsync(cancellationToken);
                if (paymentMethod == null) throw new PaymentMethodNotExistsException(request.PaymentMethodKey);
                project.PaymentMethods.Add(paymentMethod);
            }
            else
            {
                if (method is null) return Unit.Value;
                if (method.Key == Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet)
                {
                    throw new CannotDisableMosaicoWalletException();
                }
                project.PaymentMethods.Remove(method);
            }

            _projectDbContext.Projects.Update(project);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            await _cacheClient.CleanAsync(new List<string>
            {
                $"{nameof(GetProjectQuery)}_{project.Slug}",
                $"{nameof(GetProjectQuery)}_{project.Id}"
            }, cancellationToken);
            return Unit.Value;
        }
    }
}