import { Blockchain } from "../models/blockchain";

export interface GetBlockchainsResponse {
    networks: Blockchain[];
}