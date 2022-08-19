using Mosaico.Domain.Mongodb.Base.Models;

namespace Mosaico.Graph.Wallet.Entities
{
    public class Token : EntityBase
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Network { get; set; }
        public string Address { get; set; }
    }
}