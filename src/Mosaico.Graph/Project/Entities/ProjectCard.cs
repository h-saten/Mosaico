using Mosaico.Domain.Mongodb.Base.Models;
using Mosaico.Graph.Wallet.Entities;

namespace Mosaico.Graph.Project.Entities
{
    public class ProjectCard : EntityBase
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Network { get; set; }
        public string TokenId { get; set; }
        public Token Token { get; set; }
        public string CompanyId { get; set; }
    }
}