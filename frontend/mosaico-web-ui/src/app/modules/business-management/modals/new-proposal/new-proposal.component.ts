import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormDialogBase } from 'mosaico-base';
import { CompanyService } from '../../../../../../projects/mosaico-dao/src/lib/services/company.service';
import { validateForm } from '../../../../../../projects/mosaico-base/src/lib/utils/form-utils';
import { BehaviorSubject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { AddProposalCommand } from 'mosaico-dao';
import { SubSink } from 'subsink';
import { ErrorHandlingService } from '../../../../../../projects/mosaico-base/src/lib/services/error-handling.service';
import { DaoCreationHubService, Token } from 'mosaico-wallet';

@Component({
  selector: 'app-new-proposal',
  templateUrl: './new-proposal.component.html',
  styleUrls: ['./new-proposal.component.scss']
})
export class NewProposalComponent extends FormDialogBase implements OnInit, OnDestroy {
  isSubmitting$ = new BehaviorSubject<boolean>(false);
  @Input() companyId: string;
  subs = new SubSink();
  @Input() tokens: Token[] = [];

  constructor(modalService: NgbModal, private companyService: CompanyService, private translateService: TranslateService, private toastr: ToastrService,
    private errorHandlingService: ErrorHandlingService, private hub: DaoCreationHubService) { super(modalService); }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
  }

  open(): void {
    this.createForm();
    this.hub.startConnection();
    this.hub.addProposalListeners();
    super.open();
    this.modalRef.closed.subscribe((res) => {
      this.hub.removeListener();
      this.hub.resetObjects();
    });
    this.subs.sink = this.hub.proposalCreated$.subscribe((res) => {
      if (res) {
        this.translateService.get('MODALS.NEW_PROPOSAL.MESSAGES.SUCCESS').subscribe((t) => this.toastr.success(t));
        this.isSubmitting$.next(false);
        this.modalRef.close();
      }
    });
    this.subs.sink = this.hub.proposalCreationFailed$.subscribe((res) => {
      if (res) {
        this.toastr.error(res);
        this.isSubmitting$.next(false);
      }
    });
  }

  createForm(): void {
    this.form = new FormGroup({
      title: new FormControl(null, [Validators.required, Validators.minLength(5), Validators.maxLength(30)]),
      tokenId: new FormControl(null, [Validators.required]),
      description: new FormControl(null, [Validators.required, Validators.minLength(5), Validators.maxLength(500)]),
      quorumThreshold: new FormControl(20, [Validators.required, Validators.min(0), Validators.max(100)])
    });
    this.form.get('quorumThreshold').disable();
    this.subs.sink = this.isSubmitting$.subscribe((isSubmitting) => {
      if(isSubmitting) {
        this.form.disable();
      }
      else {
        this.form.enable();
        this.form.get('quorumThreshold').disable();
      }
    });
  }

  submit(): void {
    if(validateForm(this.form)) {
      this.isSubmitting$.next(true);
      const command = this.form.getRawValue() as AddProposalCommand;
      this.subs.sink = this.companyService.addProposal(this.companyId, command).subscribe((res) => {
        this.translateService.get('MODALS.NEW_PROPOSAL.MESSAGES.TRANSACTION_INITIATED').subscribe((t) => this.toastr.success(t));
      }, (error) => { this.errorHandlingService.handleErrorWithToastr(error);  this.isSubmitting$.next(false); });
    }
    else {
      this.translateService.get('MODALS.NEW_PROPOSAL.MESSAGES.INVALID_FORM').subscribe((t) => this.toastr.error(t));
    }
  }

}
