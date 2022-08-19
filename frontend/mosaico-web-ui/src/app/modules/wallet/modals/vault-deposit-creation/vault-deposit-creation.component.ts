import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { Token, Blockchain, TokenService, DaoCreationHubService, TokenDistributionService, VaultService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-vault-deposit-creation',
  templateUrl: './vault-deposit-creation.component.html',
  styleUrls: ['./vault-deposit-creation.component.scss']
})
export class VaultDepositCreationComponent  extends FormDialogBase implements OnInit, OnDestroy{

  subs = new SubSink();
  deploying = new BehaviorSubject<boolean>(false);
  @Input() token: Token;
  tokenDistributionId: string;
  networks: Blockchain[] = [];
  contractVersionToDeploy = 'vault_v1_deposit';

  constructor(modalService: NgbModal, private translateService: TranslateService,  private toastr: ToastrService,
    private errorHandler: ErrorHandlingService, private tokenService: TokenDistributionService, private vaultService: VaultService,
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

  open(distributionId: string): void {
    this.tokenDistributionId = distributionId;
    this.createForm();
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((b) => {
      this.networks = b;
    });

    this.daoHub.startConnection();
    this.daoHub.addDepositListeners();

    this.subs.sink = this.daoHub.depositCreated$.subscribe((result) => {
      if (result) {
        this.toastr.success('Deposit succesffully created');
        this.deploying.next(false);
        this.modalRef?.close(true);
      }
    });

    this.subs.sink = this.daoHub.depositCreationFailed$.subscribe((error) => {
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
    if (validateForm(this.form) && this.token && this.tokenDistributionId) {
      const wallet = this.form.get('wallet').value;
      if (wallet === 'METAMASK') {
        this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.UNSUPPORTED').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      else if (wallet === 'MOSAICO_WALLET') {
        this.deploying.next(true);
        this.subs.sink = this.vaultService.createDeposit(this.token.vault?.id, this.tokenDistributionId)
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
