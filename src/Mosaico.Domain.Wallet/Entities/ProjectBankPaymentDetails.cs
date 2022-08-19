using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class ProjectBankPaymentDetails : EntityBase
    {
        public Guid ProjectId { get; set; }
        public string Account { get; set; }
        public string BankName { get; set; }
        public string Swift { get; set; }
        public string Key { get; set; }
        public string AccountAddress { get; set; }
        public virtual List<ProjectBankTransferTitle> ProjectBankTransferTitles { get; set; } = new List<ProjectBankTransferTitle>();
    }
}