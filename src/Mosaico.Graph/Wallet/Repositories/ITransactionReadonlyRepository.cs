using Mosaico.Domain.Mongodb.Base.Abstractions;
using Mosaico.Graph.Wallet.Entities;

namespace Mosaico.Graph.Wallet.Repositories
{
    public interface ITransactionReadonlyRepository : IMongoDbBaseRepository<Transaction>
    {
        
    }
}