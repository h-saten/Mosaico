using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Mosaico.Domain.Base;
using Newtonsoft.Json;

namespace Mosaico.Domain.Wallet.Entities
{
    public class InvestorBalance
    {
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public string WalletAddress { get; set; }
        public string Network { get; set; }
    }
    
    public class Investor : EntityBase
    {
        private string _balances;
        
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
        public decimal TotalInvestment { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }

        [NotMapped]
        public List<InvestorBalance> Balances
        {
            get => string.IsNullOrWhiteSpace(_balances) ? new List<InvestorBalance>() : JsonConvert.DeserializeObject<List<InvestorBalance>>(_balances);
            set => _balances = JsonConvert.SerializeObject(value);
        }
    }
}