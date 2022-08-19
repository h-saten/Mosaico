import { BlockchainProtocolType } from './blockchain-network-protocol';

export interface SelectedTokenNetworkType {
  address: string;
  symbol: string;
  decimals: number;
  image: string;
  protocol?: BlockchainProtocolType;
}
