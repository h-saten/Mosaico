import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { ProjectService, CrowdsaleHubService } from 'mosaico-project';
import { Token, Blockchain, ActiveBlockchainService, TokenService, VaultService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { DaoCreationHubService } from '../../../../../../projects/mosaico-wallet/src/lib/services/dao-creation.hub';

@Component({
  selector: 'app-vault-deployment',
  templateUrl: './vault-deployment.component.html',
  styleUrls: ['./vault-deployment.component.scss']
})
export class VaultDeploymentComponent extends FormDialogBase implements OnInit, OnDestroy{

  subs = new SubSink();
  deploying = new BehaviorSubject<boolean>(false);
  @Input() token: Token;
  networks: Blockchain[] = [];
  contractVersionToDeploy = 'vault_v1';

  constructor(modalService: NgbModal, private translateService: TranslateService,  private toastr: ToastrService,
    private errorHandler: ErrorHandlingService, private tokenService: TokenService, private vaultService: VaultService,
    private store: Store,
    private daoHub: DaoCreationHubService) {
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

    this.daoHub.startConnection();
    this.daoHub.addVaultListeners();

    this.subs.sink = this.daoHub.vaultDeployed$.subscribe((result) => {
      if (result) {
        this.toastr.success('Vault succesffully deployed');
        this.deploying.next(false);
        this.modalRef?.close(true);
      }
    });

    this.subs.sink = this.daoHub.vaultDeploymentFailed$.subscribe((error) => {
      if (error && error.length > 0) {
        this.toastr.error(error);
        this.deploying.next(false);
      }
    });

    this.subs.sink = this.deploying.subscribe((v) => {
      if (v === true) {
        this.form.disable();
      }
      else {
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
    this.daoHub.removeListener();
    this.daoHub.resetObjects();
  }

  createForm(): void {
    this.form = new FormGroup({
      wallet: new FormControl('MOSAICO_WALLET', [Validators.required])
    });
  }

  async save(): Promise<void> {
    if (validateForm(this.form) && this.token) {
      const wallet = this.form.get('wallet').value;
      if (wallet === 'METAMASK') {
        this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.UNSUPPORTED').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      else if (wallet === 'MOSAICO_WALLET') {
        this.deploying.next(true);
        this.subs.sink = this.vaultService.createVault(this.token.id)
          .subscribe((response) => {
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

}
