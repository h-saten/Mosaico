import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';
import { BankDetailsService, UpdateBankDetailsCommand } from 'mosaico-wallet';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';

@Component({
  selector: 'app-edit-bank-data',
  templateUrl: './edit-bank-data.component.html',
  styleUrls: ['./edit-bank-data.component.scss']
})
export class EditBankDataComponent extends FormBase implements OnInit, OnDestroy {
  @Input() projectId: string;
  key: string;
  
  subs: SubSink = new SubSink();

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private toastr: ToastrService,
    private translate: TranslateService,
    private errorHandler: ErrorHandlingService,
    private bankDetailsSrvc: BankDetailsService
  ) {
    super();
   }

  ngOnInit(): void {
    this.createForm();
    this.subs.sink = this.bankDetailsSrvc.getBankDetails(this.projectId).subscribe(res => {
      if (res) {
        if(res.data)
        {
          this.key = res.data.key;
          this.form.patchValue({
            account: res.data.account,
            bankName: res.data.bankName,
            swift: res.data.swift,
            accountAddress: res.data.accountAddress
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private createForm() {
    this.form = this.formBuilder.group({
      account: ['', Validators.required],
      bankName: ['', Validators.required],
      swift: ['', Validators.required],
      accountAddress: ['', Validators.required]
    });
  }

  update() {
    if (validateForm(this.form)) {
      const command = this.form.value as UpdateBankDetailsCommand;
      command.key = this.key;
      command.projectId = this.projectId;
      this.subs.sink = this.bankDetailsSrvc.updateBankDetails(this.projectId, command).subscribe(
        _=> {
          this.toastr.success(this.translate.instant('MODALS.EDIT_BANK_DATA.MESSAGES.SUCCESS'));
          this.activeModal.close();
        },
        (error) => this.errorHandler.handleErrorWithToastr(error)
        )
    }
    else{
      this.toastr.error(this.translate.instant('MODALS.EDIT_BANK_DATA.FORM.MESSAGES.ERROR'));
    }
  }
}
