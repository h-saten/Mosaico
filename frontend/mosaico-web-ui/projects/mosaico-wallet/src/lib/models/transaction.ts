import { Token } from "./token";

export interface Transaction {
  tokenAmount: number;
  payedAmount: number;
  paymentProcessor: string;
  currency: string;
  status: string;
  initiatedAt: string;
  finishedAt: string;
  failureReason: string;
  transactionType: string;
  transactionId: string;
  transactionHash: string;
  from: string;
  to: string;
  token: Token;
  fromDisplayName: string;
  toDisplayName: string;
  transactionDirection: string;
  userId: string;
  payedInUSD: number;
  purchasedDate: string;
  tokenSymbol: string;
  extraData?: string;
  intermediateAddress?: string;
}
