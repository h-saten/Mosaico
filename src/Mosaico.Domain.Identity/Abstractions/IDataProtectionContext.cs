using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.Abstractions
{
    public interface IDataProtectionContext : IDbContext
    {
    }
}