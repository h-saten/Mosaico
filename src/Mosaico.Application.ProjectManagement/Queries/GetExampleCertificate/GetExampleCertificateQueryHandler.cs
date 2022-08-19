using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Queries.GetExampleCertificate
{
    public class GetExampleCertificateQueryHandler : IRequestHandler<GetExampleCertificateQuery, FileContentResult>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICertificateGeneratorService _certificateGenerator;

        public GetExampleCertificateQueryHandler(
            IProjectDbContext projectDbContext,
            ICertificateGeneratorService certificateGenerator)
        {
            _projectDbContext = projectDbContext;
            _certificateGenerator = certificateGenerator;
        }

        public async Task<FileContentResult> Handle(GetExampleCertificateQuery request, CancellationToken cancellationToken)
        {
            var project =
                await _projectDbContext
                    .Projects
                    .Include(t => t.InvestorCertificate.Backgrounds)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            
            if (project == null || project.TokenId == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var file = await _certificateGenerator.AsPdfAsync(x =>
            {
                x.Language = Base.Constants.Languages.English;
                x.ProjectId = request.ProjectId;
                x.Date = DateTimeOffset.UtcNow;
                x.SequenceNumber = 7;
                x.FinalizedTransactionsAmount = 12;
                x.TokensAmount = Decimal.Parse("34.56");
                x.UserName = "John Doe";
                x.TokenSymbol = "MOS";
            }, cancellationToken);
            
            return file;
        }
    }
}