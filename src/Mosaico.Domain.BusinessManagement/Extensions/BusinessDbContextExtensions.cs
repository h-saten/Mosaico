using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Extensions
{
    public static class BusinessDbContextExtensions
    {
        public static async Task<Company> GetCompanyOrThrowAsync(this IBusinessDbContext context, string identifier, CancellationToken tok = new CancellationToken())
        {
            Company company;
            if (Guid.TryParse(identifier, out var companyId))
            {
                company = await context.Companies.FirstOrDefaultAsync(p => p.Id == companyId, tok);
            }
            else
            {
                company = await context.Companies.FirstOrDefaultAsync(c => c.Slug == identifier, tok);
            }

            if (company == null)
            {
                throw new CompanyNotFoundException(identifier);
            }

            return company;
        }
        
        public static async Task<Company> GetCompanyOrThrowAsync(this IBusinessDbContext context, Guid identifier, CancellationToken tok = new CancellationToken())
        {
            var company = await context.Companies.FirstOrDefaultAsync(p => p.Id == identifier, tok);
            
            if (company == null)
            {
                throw new CompanyNotFoundException(identifier);
            }

            return company;
        }
    }
}