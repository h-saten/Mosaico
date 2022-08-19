import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { Token, Blockchain, TokenService, DaoCreationHubService, VestingService, CreateVestingCommand } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-new-vesting',
  templateUrl: './new-vesting.component.html',
  styleUrls: ['./new-vesting.component.scss']
})
export class NewVestingComponent extends FormDialogBase implements OnInit, OnDestroy {

  subs = new SubSink();
  deploying = new BehaviorSubject<boolean>(false);
  @Input() token: Token;
  networks: Blockchain[] = [];
  contractVersionToDeploy = 'vault_v1_vesting';

  constructor(modalService: NgbModal, private translateService: TranslateService,  private toastr: ToastrService,
    private errorHandler: ErrorHandlingService, private vestingService: VestingService,
    private store: Store, private daoHub: DaoCreationHubService) {
      super(modalService);
    }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
  }

  open(payload?: any): void {
    this.createForm();
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((b) => {
      this.networks = b;
    });

    this.daoHub.startConnection();
    this.daoHub.addVestingListeners();

    this.subs.sink = this.daoHub.vestingDeployed$.subscribe((result) => {
      if (result) {
        this.toastr.success('Tokens were successfully sent');
        this.deploying.next(false);
        this.modalRef?.close(true);
      }
    });

    this.subs.sink = this.daoHub.vestingDeploymentFailed$.subscribe((error) => {
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
      name: new FormControl(null, [Validators.required]),
      numberOfDays: new FormControl(null, [Validators.required, Validators.min(1), Validators.max(2000)]),
      immediatePay: new FormControl(null, [Validators.required, Validators.min(0), Validators.max(90)]),
      amountOfClaims: new FormControl(null, [Validators.required, Validators.min(1), Validators.max(24)]),
      tokenAmount: new FormControl(null, [Validators.required, Validators.min(0.1)]),
      walletAddress: new FormControl(null, [Validators.required]),
      startsAt: new FormControl(null, []),
      wallet: new FormControl('MOSAICO_WALLET', [Validators.required]),
      tokenId: new FormControl(this.token.id, [Validators.required])
    });
  }

  async save(): Promise<void> {
    if (validateForm(this.form) && this.token) {
      const command = this.form.getRawValue() as CreateVestingCommand;
      const wallet = this.form.get('wallet').value;
      if (wallet === 'METAMASK') {
        this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.UNSUPPORTED').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      else if (wallet === 'MOSAICO_WALLET') {
        this.deploying.next(true);
        this.subs.sink = this.vestingService.createVesting(command)
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
