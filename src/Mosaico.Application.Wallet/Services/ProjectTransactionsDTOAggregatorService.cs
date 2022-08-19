using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Payments.Transak.Configurations;
using Mosaico.SDK.ProjectManagement.Models;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Services
{
    public class ProjectTransactionsDTOAggregatorService : IProjectTransactionsDTOAggregatorService
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly TransakConfiguration _transakConfiguration;

        public ProjectTransactionsDTOAggregatorService(IWalletDbContext walletClient, TransakConfiguration transakConfiguration)
        {
            _walletDbContext = walletClient;
            _transakConfiguration = transakConfiguration;
        }

        public async Task<ProjectTransactionDTO> FillInDTOAsync(Transaction transaction)
        {
            var transacDTO = new ProjectTransactionDTO
            {
                TranHash = transaction.TransactionHash,
                PurchasedDate = transaction.FinishedAt ?? transaction.CreatedAt,
                Source = transaction.PaymentProcessor,
                PayedAmount = transaction.PayedAmount?.TruncateDecimals() ?? 0,
                TokenAmount = transaction.TokenAmount?.TruncateDecimals() ?? 0,
                PayedInUSD = transaction.PayedInUSD?.TruncateDecimals() ?? 0,
                UserWallet = transaction.WalletAddress,
                PaymentCurrencySymbol = transaction.PaymentCurrency?.Ticker,
                Currency = transaction.Currency,
                TransactionId = transaction.Id,
                Status = transaction.Status?.Key,
                UserId = transaction.UserId,
                FailureReason = transaction.FailureReason,
                CorrelationId = transaction.CorrelationId,
                PaymentMethod = transaction.PaymentMethod,
                ExtraData = transaction.ExtraData,
                IntermediateAddress = transaction.IntermediateAddress,
                Fee = transaction.Fee,
                ExchangeRate = transaction.ExchangeRate,
                FeePercentage = transaction.FeePercentage,
                GasFee = transaction.GasFee,
                MosaicoFee = transaction.MosaicoFee?.TruncateDecimals() ?? 0,
                TokenPrice = transaction.TokenPrice?.TruncateDecimals() ?? 0,
                SalesAgentId = transaction.SalesAgentId,
                FeeInUSD = transaction.FeeInUSD?.TruncateDecimals() ?? 0,
                MosaicoFeeInUSD = transaction.MosaicoFeeInUSD?.TruncateDecimals() ?? 0
            };

            if (transaction.PaymentProcessor == Payments.RampNetwork.Constants.PaymentProcessorName)
            {
                transacDTO.ExternalLink = $"https://api-instant.ramp.network/api/host-api/purchase/{transaction.CorrelationId}?secret={transaction.ExtraData}";
            }
            else if (transaction.PaymentProcessor == Payments.Transak.Constants.PaymentProcessorName)
            {
                transacDTO.ExternalLink = $"https://api.transak.com/api/v2/partners/order/{transaction.CorrelationId}?partnerAPISecret={_transakConfiguration.ApiSecret}";
            }

            if (transaction.TokenId.HasValue)
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId.Value);
                if (token != null)
                {
                    transacDTO.TokenSymbol = token.Symbol;
                }
            }

            return transacDTO;
        }
    }
}