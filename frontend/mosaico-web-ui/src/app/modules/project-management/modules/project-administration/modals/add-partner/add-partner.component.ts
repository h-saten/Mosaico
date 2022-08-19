import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormDialogBase, validateForm } from 'mosaico-base';
import { AffiliationService, ProjectService } from 'mosaico-project';
import { TokenService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-add-partner',
  templateUrl: './add-partner.component.html',
  styleUrls: ['./add-partner.component.scss']
})
export class AddPartnerComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  @Input() projectId: string;
  isLoading$ = new BehaviorSubject<boolean>(false);

  constructor(modalService: NgbModal, private store: Store, private toastr: ToastrService, private translateService: TranslateService, private affiliationService: AffiliationService,
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
    super.open(payload);
  }

  createForm(): void {
    this.form = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email])
    });
  }

  save(): void {
    if(validateForm(this.form)) {
      const values = this.form.getRawValue();
      this.isLoading$.next(true);
      this.subs.sink = this.affiliationService.addAffiliationPartner(this.projectId, values.email).subscribe((res) => {
        this.isLoading$.next(false);
        this.modalRef.close(true);
      }, (error) => {
        this.toastr.error(this.translateService.instant("AFFILIATION.ERR_MSG." + error.error.code));
        this.isLoading$.next(false);
      });
    }
    else {
      this.toastr.error('Form has invalid values');
    }
  }
}