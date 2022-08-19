using System.Collections.Generic;
using Mosaico.Domain.Mongodb.Base.Models;
using Mosaico.Graph.Project.Entities;

namespace Mosaico.Graph.Wallet.Entities
{
    public class Investor : EntityBase
    {
        public User.Entities.User User { get; set; }
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
        public List<string> ProjectIds { get; set; } = new List<string>();
        public List<ProjectCard> Projects { get; set; } = new List<ProjectCard>();
        public List<WalletBalance> Balances { get; set; } = new List<WalletBalance>();
    }
}