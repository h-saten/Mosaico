using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class ProjectBankPaymentDetailsDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Account { get; set; }
        public string BankName { get; set; }
        public string Swift { get; set; }
        public string Key { get; set; }
        public string AccountAddress { get; set; }
        public string Reference { get; set; }
    }
}