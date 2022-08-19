using Mosaico.Domain.Mongodb.Base.Abstractions;
using Mosaico.Domain.Mongodb.Base.Repository;
using Mosaico.Graph.Wallet.Entities;

namespace Mosaico.Graph.Wallet.Repositories
{
    public class TransactionMongodbRepository : MongoDbBaseRepository<Transaction>, ITransactionReadonlyRepository
    {
        public TransactionMongodbRepository(IMongoDBContext context) : base(context)
        {
        }
    }
}