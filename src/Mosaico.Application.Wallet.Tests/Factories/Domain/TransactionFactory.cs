using System;
using FizzWare.NBuilder;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class TransactionFactory
    {
        public static Transaction Create(
            string userId, 
            Guid tokenId,
            TransactionType type,
            TransactionStatus status)
        {
            var transaction = Builder<Transaction>.CreateNew().Build();
            transaction.Id = Guid.NewGuid();
            transaction.Status = status;
            transaction.Type = type;
            transaction.TokenId = tokenId;
            transaction.UserId = userId;
            transaction.ConfirmationAttemptsCounter = 0;
            
            var paymentCurrency = Builder<PaymentCurrency>.CreateNew().Build();
            paymentCurrency.Id = Guid.NewGuid();
            transaction.PaymentCurrency = paymentCurrency;
            return transaction;
        }

        public static Transaction CreateTransaction(
            this IWalletDbContext context,
            string userId, 
            Guid tokenId,
            TransactionType type,
            TransactionStatus status)
        {
            var transaction = Create(userId, tokenId, type, status);
            context.Transactions.Add(transaction);
            return transaction;
        }

        public static Transaction CreateConfirmedPurchaseTransaction(
            this IWalletDbContext context,
            string userId, 
            Guid tokenId)
        {
            var transactionStatusConfirmed = TransactionStatusFactory.Confirmed();
            var transactionType = TransactionTypeFactory.Purchase();
            var transaction = Create(userId, tokenId, transactionType, transactionStatusConfirmed);
            transaction.FailureReason = null;
            context.Transactions.Add(transaction);
            return transaction;
        }
    }
}