import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Blockchain, Token, TokenService, TOKEN_PERMISSIONS } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { ErrorHandlingService } from '../../../../../../projects/mosaico-base/src/lib/services/error-handling.service';
import { selectCurrentActiveBlockchains } from '../../../../store/selectors';
import { clearToken, selectTokenPermissions, setToken } from '../../store';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BlockchainService, DEFAULT_MODAL_OPTIONS, FileParameters } from 'mosaico-base';
import { EditTokenLogoComponent } from '../../modals';

@Component({
  selector: 'app-token-management',
  templateUrl: './token-management.component.html',
  styleUrls: ['./token-management.component.scss']
})
export class TokenManagementComponent implements OnInit, OnDestroy {
  @Input() tokenId: string;
  token: Token;
  isLoading = false;
  networks: Blockchain[] = [];
  subs = new SubSink();
  networkLogoUrl: string;
  canEdit = false;
  link = '';
  chains: Blockchain[] = [];

  constructor(private route: ActivatedRoute, private store: Store, private tokenSevice: TokenService, private toastr: ToastrService, private errorHandler: ErrorHandlingService,
    private modalService: NgbModal, private blockchainService: BlockchainService) {
    this.route.paramMap.subscribe( paramMap => {
      const paramTokenId = paramMap.get('tokenId');
      if(paramTokenId && paramTokenId.length > 0){
        this.tokenId = paramMap.get('tokenId');
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.store.dispatch(clearToken());
  }

  navigateToEtherscan(): void {
    if(this.link && this.link.length > 0) {
      window.open(this.link, "_blank", 'noopener');
    }
  }

  getToken(): void {
    this.isLoading = true;
    if(this.tokenId) {
      this.subs.sink = this.tokenSevice.getToken(this.tokenId).subscribe((res) => {
        if(res && res.data) {
          this.token = res.data;
          this.networkLogoUrl = this.networks?.find((n) => n.name === this.token.network)?.logoUrl;
          const chain = this.chains.find((c) => c.name === this.token.network);
          if(chain){
            this.link = this.blockchainService.getTokenLink(this.token.address, chain.etherscanUrl);
          }
          this.store.dispatch(setToken({token: this.token}));
        }
        this.isLoading = false;
      }, (error) => { this.errorHandler.handleErrorWithRedirect(error, '/')});
    }
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((b) => {
      this.networks = b;
      this.getToken();
    });
    this.subs.sink = this.store.select(selectTokenPermissions).subscribe((res) => {
      this.canEdit = res && res[TOKEN_PERMISSIONS.CAN_EDIT] === true;
    });
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((chains) => {
      this.chains = chains;
    });
  }

  onCopied(): void {
    this.toastr.success('Address copied');
  }

  openLogoEditingModal(): void {
    if(this.canEdit && this.token) {
      const modalRef = this.modalService.open(EditTokenLogoComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.currentImgUrl = this.token.logoUrl;
      modalRef.componentInstance.tokenId = this.token.id;
      this.subs.sink = modalRef.closed.subscribe((result: FileParameters) => {
        if(result){
          setTimeout(() => {
            this.getToken();
          }, 2000);
        }
      });
    }
  }

}
