using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.EstimateGasJob, IsRecurring = true)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class EstimateContractDeploymentJob : HangfireBackgroundJobBase
    {
        private readonly ILogger _logger;
        private readonly IEstimateDispatcher _estimateDispatcher;
        private readonly ITimeSeriesRepository _seriesRepository;
        private readonly IGasEstimator _deploymentEstimator;

        public EstimateContractDeploymentJob(IEstimateDispatcher estimateDispatcher, ITimeSeriesRepository seriesRepository, IGasEstimator deploymentEstimator, ILogger logger = null)
        {
            _estimateDispatcher = estimateDispatcher;
            _seriesRepository = seriesRepository;
            _deploymentEstimator = deploymentEstimator;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            var estimates = new List<DeploymentEstimateDTO>();
            
            var deploymentEstimates = await _deploymentEstimator.EstimateDeploymentAsync();
            estimates.AddRange(deploymentEstimates);
            
            var transferEstimates = await _deploymentEstimator.EstimateTransferAsync("USDT");
            estimates.AddRange(transferEstimates);

            var nativeTransferEstimates = await _deploymentEstimator.EstimateTransferAsync();
            estimates.AddRange(nativeTransferEstimates);
            
            var mosaicoDeploymentEstimates = await GetMosaicoEstimatesAsync(estimates);
            estimates.AddRange(mosaicoDeploymentEstimates);
            
            await _seriesRepository.AddAsync(Domain.Wallet.Constants.RedisKeys.Estimates,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(), estimates);
            await _estimateDispatcher.DispatchAsync(estimates);
        }

        private async Task<List<DeploymentEstimateDTO>> GetMosaicoEstimatesAsync(List<DeploymentEstimateDTO> existingEstimates)
        {
            var estimates = new List<DeploymentEstimateDTO>();
            //var transferEstimate = await _deploymentEstimator.EstimateTransferAsync();
            foreach (var estimate in existingEstimates.Where(es => es.PaymentMethod == Domain.Wallet.Constants.DeploymentPaymentMethods.Metamask))
            {
                //var networkTransferEstimate = transferEstimate.FirstOrDefault(n => n.Network == estimate.Network);
                // if (networkTransferEstimate != null)
                // {
                    // var priceWithoutFee = estimate.Price + networkTransferEstimate.Price;
                    // var fee = priceWithoutFee * 0.01m;
                    
                    estimates.Add(new DeploymentEstimateDTO
                    {
                        Currency = estimate.Currency,
                        Gas = estimate.Gas,
                        Network = estimate.Network,
                        ContractVersion = estimate.ContractVersion,
                        GasPrice = estimate.GasPrice,
                        EstimatedAt = estimate.EstimatedAt,
                        PaymentMethod = Domain.Wallet.Constants.DeploymentPaymentMethods.Mosaico,
                        Price = estimate.Price,
                        Fee = estimate.Fee,
                        NativeCurrencyAmount = estimate.NativeCurrencyAmount,
                        NativeCurrencyTicker = estimate.NativeCurrencyTicker
                    });
                //}
            }

            return estimates;
        }
    }
}