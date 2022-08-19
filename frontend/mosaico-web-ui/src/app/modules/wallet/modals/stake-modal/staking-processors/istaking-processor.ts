import { StakingPair, WalletStake } from "mosaico-wallet";

export interface IStakingProcessor {
    stake(stakingPair: StakingPair, balance: number, days: number): Promise<string|boolean>;
    approve(stakingPair: StakingPair, balance: number, days: number): Promise<string|boolean>;
    withdraw(stake: WalletStake): Promise<string|boolean>;
}