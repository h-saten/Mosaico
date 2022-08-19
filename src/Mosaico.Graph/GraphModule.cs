using Autofac;
using Mosaico.Graph.Wallet.Repositories;

namespace Mosaico.Graph
{
    public class GraphModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<TransactionMongodbRepository>().As<ITransactionReadonlyRepository>();
        }
    }
}