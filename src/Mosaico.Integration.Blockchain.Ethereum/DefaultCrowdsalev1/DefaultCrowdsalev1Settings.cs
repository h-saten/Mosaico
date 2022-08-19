using System.Collections.Generic;
using System.Numerics;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1
{
    public class DefaultCrowdsalev1Settings
    {
        public static Dictionary<string, object> GetSettings(string ownerAddress, string ERC20Address, BigInteger softCapDenominator, List<string> supportedStableCoins, BigInteger numberOfStages)
        {
            return new Dictionary<string, object>
            {
                {"wallet", ownerAddress},
                {"token", ERC20Address},
                {"softCapDenominator", softCapDenominator},
                {"supportedStablecoins", supportedStableCoins},
                {"numberOfStages_", numberOfStages}
            };
        }

        public static StartNextStageFunction GetStartNextStageFunction(
            string name, 
            BigInteger cap, 
            BigInteger nativeCurrencyRate,
            BigInteger stableCoinRate,
            bool isPrivate, 
            BigInteger minIndividualCap, 
            BigInteger maxIndividualCap, 
            List<string> whitelisted)
        {
            return new StartNextStageFunction
            {
                Settings = new StageSettings
                {
                    Cap = cap,
                    Name = name,
                    Rate = nativeCurrencyRate,
                    IsPrivate = isPrivate,
                    MaxIndividualCap = maxIndividualCap,
                    MinIndividualCap = minIndividualCap,
                    Whitelist = whitelisted
                },
                StableCoinRate_ = stableCoinRate
            };
        }

        public static BuyTokensFunction BuyTokensFunction(string beneficiaryWalletAddress, BigInteger etherAmount)
        {
            return new BuyTokensFunction
            {
                Beneficiary = beneficiaryWalletAddress,
                AmountToSend = etherAmount,
                Gas = 1200000
            };
        }
    }
}