import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { ProjectService, CrowdsaleHubService } from 'mosaico-project';
import { Token, Blockchain, DeploymentEstimateHubService, ActiveBlockchainService, SystemWalletService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-stage-deployment',
  templateUrl: './stage-deployment.component.html',
  styleUrls: ['./stage-deployment.component.scss']
})
export class StageDeploymentComponent extends FormDialogBase implements OnInit, OnDestroy {

  subs = new SubSink();
  deploying = new BehaviorSubject<boolean>(false);
  @Input() token: Token;
  @Input() projectId: string;
  @Input() stageId: string;
  networks: Blockchain[] = [];
  contractVersionToDeploy = 'stage_deploy_v1';

  constructor(modalService: NgbModal, private translateService: TranslateService, private toastr: ToastrService,
    private errorHandler: ErrorHandlingService, private hub: DeploymentEstimateHubService, private projectService: ProjectService,
    private store: Store, private activeBlockchain: ActiveBlockchainService, private systemWalletService: SystemWalletService,
    private crowdsaleHub: CrowdsaleHubService) {
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

    this.crowdsaleHub.startConnection();
    this.crowdsaleHub.addStageListeners();

    this.subs.sink = this.crowdsaleHub.stageDeployed$.subscribe((result) => {
      if (result) {
        this.subs.sink = this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.STAGE_DEPLOY_SUCCESS').subscribe((t) => {
          this.toastr.success(t);
          this.deploying.next(false);
        });
        this.modalRef?.close(true);
      }
    });

    this.subs.sink = this.crowdsaleHub.stageDeploymentFailed$.subscribe((payload) => {
      if (payload?.error && payload.error.length > 0) {
        this.toastr.error(payload.error);
        this.deploying.next(false);
      }
    });

    this.hub.startConnection();
    this.hub.addListener();

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
    this.hub.removeListener();
    this.hub.resetObjects();
    this.crowdsaleHub.removeListener();
    this.crowdsaleHub.resetObjects();
  }

  createForm(): void {
    this.form = new FormGroup({
      wallet: new FormControl('MOSAICO_WALLET', [Validators.required])
    });
  }

  async save(): Promise<void> {
    if (validateForm(this.form) && this.stageId && this.projectId) {
      const wallet = this.form.get('wallet').value;
      if (wallet === 'METAMASK') {
        this.translateService.get('TOKEN_MANAGEMENT.MESSAGES.UNSUPPORTED').subscribe((t) => {
          this.toastr.error(t);
        });
      }
      else if (wallet === 'MOSAICO_WALLET') {
        this.deploying.next(true);
        this.subs.sink = this.projectService.deployStage(this.projectId, this.stageId)
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
