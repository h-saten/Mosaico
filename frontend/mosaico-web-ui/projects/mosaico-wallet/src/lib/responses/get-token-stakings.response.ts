import {TokenStaking} from "../models/token-staking";

export interface GetTokenStakingsResponse {
  stakings: TokenStaking[];
  tokenSymbol: string;
}
