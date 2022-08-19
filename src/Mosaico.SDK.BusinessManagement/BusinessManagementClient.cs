using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.SDK.Base.Exceptions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.BusinessManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.BusinessManagement.Exceptions;
using AutoMapper;

namespace Mosaico.SDK.BusinessManagement
{
    public class BusinessManagementClient : IBusinessManagementClient
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public BusinessManagementClient(IBusinessDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<MosaicoCompany> GetCompanyAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            var company = await _context.Companies.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: token);
            if (company == null)
            {
                //TODO: if API integration than exception should be filled from object response
                throw new MosaicoException($"Company {id} not found", "COMPANY_NOT_FOUND", 404);
            }

            //TODO: introduce Automapper here
            return _mapper.Map<MosaicoCompany>(company);
        }

        public async Task DeleteUserMembershipAsync(string userId, CancellationToken token = new CancellationToken())
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var companyTeamMembers = await _context.TeamMembers.Where(x => x.UserId == userId).ToListAsync(cancellationToken: token);
                    foreach (var ctm in companyTeamMembers)
                    {
                        _context.TeamMembers.Remove(ctm);
                    }
                    await _context.SaveChangesAsync(cancellationToken: token);

                    await transaction.CommitAsync(cancellationToken: token);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken: token);
                    throw;
                }
            }
        }

        public async Task SetContractAddressAsync(Guid id, string contractAddress, CancellationToken token = new CancellationToken())
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id, token);
            if (company == null)
            {
                throw new CompanyNotFoundException(id);
            }

            company.ContractAddress = contractAddress;
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteCompanyAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id, token);
            if (company == null)
            {
                throw new CompanyNotFoundException(id);
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync(token);
        }
    }
}
