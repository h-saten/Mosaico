using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Commands.ApproveVerificationRequest
{
    public class ApproveVerificationRequestCommandHandler : IRequestHandler<ApproveVerificationRequestCommand, Guid>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;

        public ApproveVerificationRequestCommandHandler(IBusinessDbContext dbContext, ILogger logger = null)
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(ApproveVerificationRequestCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to approve a company verification request");
                    var verification = await _context.Verifications.FirstOrDefaultAsync(x=>x.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
                    if (verification == null)
                    {
                        throw new VerificationNotFoundException(Guid.Empty);
                    }
                    var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken: cancellationToken);
                    if (company == null)
                    {
                        throw new CompanyNotFoundException(request.CompanyId);
                    }
                    company.IsApproved = true;
                    _context.Verifications.Remove(verification);
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Verification was successfully approved and deleted");
                    await transaction.CommitAsync(cancellationToken);
                    return verification.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
       
    }
}