using System;
using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.Wallet.DTO
{
    public class ProjectWalletBalanceDTO
    {
        public Guid Id { get; set; }
        public ProjectInvestorDTO User { get; set; }
        public string Address { get; set; }
        public List<TokenBalanceDTO> Balances { get; set; } = new List<TokenBalanceDTO>();
        public decimal TotalInvestment { get; set; }
    }
}