import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { BehaviorSubject, zip } from 'rxjs';
import { AirdropService, CreateAirdropCommand, ProjectService, Stage } from 'mosaico-project';
import { CompanyWalletService } from 'mosaico-wallet';
import { Store } from '@ngrx/store';
import { selectProjectPreview } from '../../../../store/project.selectors';

@Component({
  selector: 'app-create-airdrop',
  templateUrl: './create-airdrop.component.html',
  styleUrls: ['./create-airdrop.component.scss']
})
export class CreateAirdropComponent extends FormDialogBase implements OnInit {
  @Input() projectId: string;
  @Input() companyId: string;
  @Input() tokenId: string;
  tokenBalance = 0;
  isLoading$ = new BehaviorSubject<boolean>(false);
  isSaving$ = new BehaviorSubject<boolean>(false);
  subs = new SubSink();
  stages: Stage[] = [];
  displayStageControl = false;

  constructor(modal: NgbModal, private translateService: TranslateService, private toastrService: ToastrService, private service: AirdropService,
    private walletService: CompanyWalletService, private store: Store, private projectService: ProjectService,
    private errorHandler: ErrorHandlingService) { super(modal); }

  ngOnInit(): void {
    this.createForm();
    this.isSaving$.subscribe((v) => {
      if (v === true) {
        this.form.disable();
      }
      else {
        this.form.enable();
      }
    });
  }

  createForm(): void {
    this.form = new FormGroup({
      name: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
      startDate: new FormControl(null, [Validators.required]),
      endDate: new FormControl(null, [Validators.required]),
      totalCap: new FormControl(null, [Validators.required, Validators.min(0.1)]),
      tokensPerParticipant: new FormControl(null, [Validators.required, Validators.min(0.1)]),
      isOpened: new FormControl(true),
      countAsPurchase: new FormControl(false),
      stageId: new FormControl(null)
    });
    this.form.get('countAsPurchase').valueChanges.subscribe((res) => {
      this.displayStageControl = res;
    });
  }

  open(): void {
    this.createForm();
    super.open();
    this.loadData();
  }

  loadData(): void {
    this.isLoading$.next(true);
    this.subs.sink = zip(this.walletService.getCompanyWalletTokens(this.companyId), this.projectService.getProjectStages(this.projectId)).subscribe((responses) => {
      const tokens = responses[0]?.data?.tokens;
      const token = tokens?.find((t) => t.id === this.tokenId);
      if (token) {
        this.tokenBalance = token.balance;
      }
      this.stages = responses[1]?.data?.stages;
      this.isLoading$.next(false);
    });
  }

  save(): void {
    if (validateForm(this.form)) {
      const command = this.form.getRawValue() as CreateAirdropCommand;
      this.isSaving$.next(true);
      this.service.create(this.projectId, command).subscribe((res) => {
        this.toastrService.success('Airdrop was created');
        this.modalRef.close(true);
        this.isSaving$.next(false);
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isSaving$.next(false); });
    }
    else {
      this.toastrService.error(this.translateService.instant('CREATE_AIRDROP.FORM.VALIDATORS.INVALID_VALUES'));
    }
  }
}
