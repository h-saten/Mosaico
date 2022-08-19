using System;
using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Base;

namespace Mosaico.Application.Wallet.Queries.GetProjectWallet
{
    public class GetProjectWalletQueryResponse : PaginatedList<ProjectWalletBalanceDTO>
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
    }
}