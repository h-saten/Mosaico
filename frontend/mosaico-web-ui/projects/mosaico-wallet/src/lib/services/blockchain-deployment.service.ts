import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BlockchainService, ConfigService } from 'mosaico-base';
import Web3 from 'web3';
import { MosaicoERC20v1ConstructorParameters } from '../smart_contracts/mosaico_erc20_v1';
import { BlockchainNetworkType } from '../../../../mosaico-base/src/lib/models/blockchain-network-type';
import { Blockchain } from '../models';
import { ActiveBlockchainService } from './active-blockchain.service';
import erc20_v1 from '../smart_contracts/abi/MosaicoERC20v1.json';
import crowdsale_v1 from '../smart_contracts/abi/DefaultCrowdsalev1.json';
import staking_v1 from '../smart_contracts/abi/Staking.json';

export type ERC20ContractVersion = 'erc20_v1';
export type CrowdsaleContractVersion = 'crowdsale_v1';
export type StaknigContractVersion = 'staking_v1';

export const ERC20_CONTRACT_VERSION_MAP: { [key: string]: any } = {
    'erc20_v1': erc20_v1
};

export const CROWDSALE_CONTRACT_VERSION_MAP: { [key: string]: any } = {
    'crowdsale_v1': crowdsale_v1
};

export const STAKING_CONTRACT_VERSION_MAP: { [key: string]: any } = {
    'staking_v1': staking_v1
};

@Injectable({
    providedIn: 'root'
})
export class BlockchainDeploymentService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService, private blockchainService: BlockchainService, private activeBlockchainService: ActiveBlockchainService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    async deployERC20(version: ERC20ContractVersion, network: BlockchainNetworkType, args: MosaicoERC20v1ConstructorParameters, from: string): Promise<string> {
        const contract = ERC20_CONTRACT_VERSION_MAP[version];
        if (contract) {
            let user = await this.blockchainService.getCurrentWallet();
            if (!user) {
                user = await this.blockchainService.authenticateToMetamask();
                await this._deployERC20(contract, network, args, from);
            }
            else {
                return this._deployERC20(contract, network, args, from);
            }
        }
        else {
            throw Error("Invalid contract version");
        }
    }

    private _deployERC20(contract: any, network: BlockchainNetworkType, args: any, from: string): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            this.activeBlockchainService.getActiveBlockchains().subscribe((res) => {
                if (res?.data) {
                    const blockchain = res.data.networks?.find((b) => b.name === network);
                    if (blockchain) {
                        this._deployABI(contract, blockchain, args, from).then((r) => {
                            resolve(r);
                        }, (error) => { reject(error); });
                    }
                    else{
                        reject("Invalid blockchain was supplied to contract deployment");
                    }
                }
                else {
                    reject("No active blockchains returned from backend");
                }
            }, (error) => { reject(error); });
        });
    }

    private async _deployABI(contractMetadata: any, chain: Blockchain, args: any, from: string): Promise<string> {
        const web3 = new Web3(Web3.givenProvider);
        const contract = new web3.eth.Contract(contractMetadata.abi);
        const sendTransaction = contract.deploy({
            data: contractMetadata.bytecode,
            arguments: [args]
        });
        
        const estimatedGas = await sendTransaction.estimateGas({from});
        const estimatedGasPrice = await web3.eth.getGasPrice();

        let options = {
            data: sendTransaction.encodeABI(),
            gas : estimatedGas,
            from,
            chainId: +chain.chainId,
            gasPrice: estimatedGasPrice
        };
        const result = await web3.eth.sendTransaction(options);
        return result.contractAddress;
    }
}