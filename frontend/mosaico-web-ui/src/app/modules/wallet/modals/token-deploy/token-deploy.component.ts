import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainService, ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { ActiveBlockchainService, Blockchain, BlockchainDeploymentService, DeploymentEstimate, DeploymentEstimateHubService, ERC20ContractVersion, SystemWallet, SystemWalletService, Token, TokenService, WalletHubService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectMetamaskChainId } from '../../store';
import { BehaviorSubject } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import Web3 from 'web3';
import { BlockchainNetworkType } from '../../../../../../projects/mosaico-base/src/lib/models/blockchain-network-type';
import { selectCurrentActiveBlockchains } from '../../../../store/selectors';

@Component({
  selector: 'app-token-deploy',
  templateUrl: './token-deploy.component.html',
  styleUrls: ['./token-deploy.component.scss']
})
export class TokenDeployComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  contractVersionToDeploy: ERC20ContractVersion = 'erc20_v1';
  deploying = new BehaviorSubject<boolean>(false);
  @Input() token: Token;
  networks: Blockchain[] = [];

  constructor(modalService: NgbModal, private tokenService: TokenService, private toastr: ToastrService, private errorHandler: ErrorHandlingService,
    private translateService: TranslateService, private hub: DeploymentEstimateHubService,
    private store: Store, private activeBlockchain: ActiveBlockchainService, private systemWalletService: SystemWalletService,
    private walletHub: WalletHubService) {
      super(modalService);
      this.extraOptions = {
        modalDialogClass: "mosaico-payment-modal"
      };
    }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
  }

  open(): void {
    this.createForm();
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((b) => {
      this.networks = b;
    });
    this.walletHub.startConnection();
    this.walletHub.addListener();
    this.subs.sink = this.walletHub.deployed$.subscribe((result) => {
      if(result && result.tokenId === this.token.id){
        this.subs.sink = this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.SUCCESS').subscribe((t) => {
          this.toastr.success(t);
          this.deploying.next(false);
        });
        this.modalRef.close(true);
      }
    });
    this.subs.sink = this.walletHub.failed$.subscribe((error) => {
      if(error && error.length > 0) {
        this.toastr.error(error);
        this.deploying.next(false);
      }
    });
    this.subs.sink = this.deploying.subscribe((v) => {
      if(v === true) {
        this.form.disable();
      }
      else{
        this.form.enable();
      }
    });
    super.open();
    this.modalRef.dismissed.subscribe(() => {
      this.stopHubs();
      this.subs.unsubscribe();
    });
    this.modalRef.closed.subscribe(() => {
      this.stopHubs();
      this.subs.unsubscribe();
    });
  }

  private stopHubs(): void {
    this.walletHub.removeListener();
    this.walletHub.resetObjects();
  }

  private createForm(): void {
    this.form = new FormGroup({
      wallet: new FormControl('MOSAICO_WALLET', [Validators.required])
    });
  }

  async save(): Promise<void> {
    if (validateForm(this.form)) {
      const wallet = this.form.get('wallet').value;
      if(wallet === 'METAMASK') {
        this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.UNSUPPORTED').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      else if(wallet === 'MOSAICO_WALLET'){
        this.deploying.next(true);
        this.subs.sink = this.tokenService.deploy(this.token.id).subscribe((response) => {
          this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.TRANSACTION_INITIATED').subscribe((t) => {
            this.toastr.success(t);
          });
        }, (error) => { this.deploying.next(false); this.errorHandler.handleErrorWithToastr(error); });
      }
    }
    else {
      this.subs.sink = this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastr.error(res);
      });
    }
  }

  // private async deploySmartContract(wallet: string, contractVersion: ERC20ContractVersion): Promise<void> {
  //   try {
  //     const contractAddress = await this.deploymentService.deployERC20(contractVersion, this.token.network, {
  //       initialSupply: Web3.utils.toWei(this.token.totalSupply.toString()),
  //       isBurnable: this.token.isBurnable,
  //       isMintable: this.token.isMintable,
  //       isPaused: false,
  //       name: this.token.name,
  //       symbol: this.token.symbol,
  //       walletAddress: wallet,
  //       url: 'moisaico.ai',
  //       isGovernance: this.token.isGovernance
  //     }, wallet);
  //     await this.updateToken(wallet, contractAddress);
  //   }
  //   catch (error) {
  //     const message = error?.message ?? "Error occured during deployment";
  //     this.toastr.error(message);
  //     this.deploying.next(false);
  //   }
  // }

  // private async updateToken(ownerAddress: string, contractAddress: string): Promise<void> {
  //   await this.tokenService.update(this.token.id, {
  //     contractAddress,
  //     contractVersion: this.contractVersionToDeploy,
  //     ownerAddress
  //   }).toPromise();
  //   this.subs.sink = this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.SUCCESS').subscribe((t) => {
  //     this.toastr.success(t);
  //     this.deploying.next(false);
  //   });
  //   this.modalRef.close(true);
  // }

  // private async processesMetamask(): Promise<void> {
  //   try {
  //     this.deploying.next(true);
  //     const network = this.networks.find((n) => n.name === this.token?.network);
  //     if (network == null) {
  //       throw new Error("Network not found");
  //     }
  //     if(network.chainId !== this.currentChainId) {
  //       throw new Error("Selected network does not match network in metamask");
  //     }

  //     let user = await this.blockchainService.getCurrentWallet();
  //     if (!user) {
  //       user = await this.blockchainService.authenticateToMetamask();
        
  //     }
  //     await this.deploySmartContract(user, this.contractVersionToDeploy);
  //   }
  //   catch (error) {
  //     if (error?.message) {
  //       this.toastr.error(error.message);
  //     }
  //     else {
  //       this.toastr.error(error);
  //     }
  //     this.deploying.next(false);
  //   }
  // }

}
