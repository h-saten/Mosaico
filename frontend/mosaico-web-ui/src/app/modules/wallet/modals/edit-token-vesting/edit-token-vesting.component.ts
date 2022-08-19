import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { FormDialogBase, ErrorHandlingService, validateForm } from 'mosaico-base';
import { Token, TokenService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-edit-token-vesting',
  templateUrl: './edit-token-vesting.component.html',
  styleUrls: ['./edit-token-vesting.component.scss']
})
export class EditTokenVestingComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  @Input() token: Token;
  isLoading$ = new BehaviorSubject<boolean>(false);

  constructor(modalService: NgbModal, private store: Store, private toastr: ToastrService, private translateService: TranslateService, private tokenService: TokenService,
    private errorHandler: ErrorHandlingService) {
    super(modalService);
  }

  ngOnDestroy(): void {

  }

  ngOnInit(): void {
    this.createForm();
  }

  open(payload?: any): void {
    this.createForm();
    if(this.token) {
      this.form?.setValue({
        isEnabled: this.token.isVestingEnabled
      });
    }
    super.open(payload);
  }

  createForm(): void {
    this.form = new FormGroup({
      isEnabled: new FormControl(false)
    });
  }

  save(): void {
    if(validateForm(this.form)) {
      const values = this.form.getRawValue();
      this.isLoading$.next(true);
      this.subs.sink = this.tokenService.enableVesting(this.token.id, {
          isEnabled: values.isEnabled
      }).subscribe((res) => {
        this.isLoading$.next(false);
        this.modalRef.close(true);
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
        this.isLoading$.next(false);
      });
    }
    else {
      this.toastr.error('Form has invalid values');
    }
  }

}
