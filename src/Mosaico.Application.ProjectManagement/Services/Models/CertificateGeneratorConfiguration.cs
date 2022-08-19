using System;

namespace Mosaico.Application.ProjectManagement.Services.Models
{
    public class CertificateGeneratorConfiguration
    {
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
        public decimal TokensAmount { get; set; }
        public string TokenSymbol { get; set; }
        public DateTimeOffset Date { get; set; }
        public int FinalizedTransactionsAmount { get; set; }
        public int SequenceNumber { get; set; }
    }
}