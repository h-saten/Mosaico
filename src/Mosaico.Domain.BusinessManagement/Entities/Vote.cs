using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class Vote : EntityBase
    {
        public string VotedByAddress { get; set; }
        public VoteResult Result { get; set; }
        public decimal Tokens { get; set; }
        public virtual Proposal Proposal { get; set; }
        public Guid ProposalId { get; set; }
        public string TransactionHash { get; set; }
    }
}