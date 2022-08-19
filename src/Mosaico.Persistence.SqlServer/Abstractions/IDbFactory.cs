using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mosaico.Persistence.SqlServer.Abstractions
{
    public interface IDbFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
    {
        DbContextOptionsBuilder<T> GetOptions(DbContextOptions<T> options = null);
        void SetConnectionString(string connectionString);
    }
}