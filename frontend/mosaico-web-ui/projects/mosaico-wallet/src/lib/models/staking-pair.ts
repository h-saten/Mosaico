import { PaymentCurrency } from "./payment-currency";
import { Token } from "./token";

export interface StakingPair {
    id: string;
    token: Token;
    stakingToken: Token;
    contractAddress: string;
    canChangeStakingPeriod: boolean;
    minimumDaysToStake: number;
    network: string;
    estimatedAPR: number;
    estimatedRewardInUSD: number;
    paymentCurrencies: string[];
    stakingPaymentCurrency: PaymentCurrency;
    type: string;
    version: string;
    stakingRegulation: string;
    termsAndConditionsUrl: string;
}