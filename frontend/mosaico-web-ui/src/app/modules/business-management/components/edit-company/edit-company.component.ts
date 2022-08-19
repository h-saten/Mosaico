import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { selectCompanyPermissions, selectCompanyPreview } from '../../store/business.selectors';
import { COMPANY_PERMISSIONS } from '../../constants';
import { Country, CountryService, ErrorHandlingService, trim, validateForm } from 'mosaico-base';
import { Company, CompanyService } from 'mosaico-dao';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.scss']
})
export class EditCompanyComponent implements OnInit, OnDestroy {
  mainForm: FormGroup;
  active = 1;
  canEdit = false;
  isApproved = false;
  isCurrentFormValid = false;
  private sub: SubSink = new SubSink();
  countries: Country[] = [];
  sizeOptions = [
    { value: 'S', key: 'COMPANY_EDIT.FORM.SIZE.OPTIONS.SMALL' },
    { value: 'M', key: 'COMPANY_EDIT.FORM.SIZE.OPTIONS.MEDIUM'},
    { value: 'L', key: 'COMPANY_EDIT.FORM.SIZE.OPTIONS.LARGE'}
  ];
  currentCompanyId: string;

  constructor(
    private formBuilder: FormBuilder,
    private companyService: CompanyService,
    private store: Store,
    private toastr: ToastrService,
    private errorHandling: ErrorHandlingService,
    private translateService: TranslateService,
    private router: Router,
    private countryService: CountryService
  ) {

  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
    this.sub.sink = this.countryService.getCountries().subscribe((res) => {
      this.countries = res?.data;
    });
    this.sub.sink = this.store.select(selectCompanyPreview).subscribe((company) => {
      if (company) {
        this.currentCompanyId = company.id;
        this.updateFormValue(company);
        this.isApproved = company.isApproved;
        this.store.select(selectCompanyPermissions).subscribe((res) => {
          this.canEdit = res && res[COMPANY_PERMISSIONS.CAN_EDIT_DETAILS];
        });
      }
      else {
        this.router.navigateByUrl('/dao');
      }
    });
  }

  private loadCompany(): void {
    this.sub.sink = this.companyService.getCompany(this.currentCompanyId).subscribe((response) => {
      if (response && response.data) {
        this.updateFormValue(response.data.company);
        this.isApproved = response.data.company.isApproved;
      }
    });
  }

  private updateFormValue(company: Company): void {
    this.mainForm.setValue({
      companyName: company.companyName,
      companyDescription: company.companyDescription,
      country: company.country,
      street: company.street,
      postalCode: company.postalCode,
      vatid: company.vatId,
      size: company.size,
      email: company.email,
      phoneNumber: company.phoneNumber,
      region: company.region
    });
  }

  private createForm(): void {
    this.mainForm = this.formBuilder.group({
      companyName: [''],
      companyDescription:[''],
      country: [null],
      street: [''],
      postalCode: [''],
      vatid: [''],
      size: [null],
      phoneNumber: [''],
      email: [''],
      region: ['']
    });
    this.mainForm.get('companyName').disable();
  }

  disableForm(): void {
    this.mainForm.disable();
  }

  enableForm(): void {
    this.mainForm.enable();
    this.mainForm.get('companyName').disable();
  }

  // leaveCompany(): void {
  //   this.sub.sink = this.companyService.leaveCompany(this.currentCompanyId).subscribe((result) => {
  //     if (result && result.ok) {
  //       this.translateService.get('COMPANY_EDIT.MESSAGES.LEFT').subscribe((t) => {
  //         this.toastr.success(t);
  //       });
  //       this.router.navigateByUrl('/dao/my');
  //     } else {
  //       this.toastr.error(result);
  //     }
  //   }, (error) => {
  //     this.errorHandling.handleErrorWithToastr(error);
  //   });
  // }

  setCurrentFormValidated(valid: boolean): void {
    this.isCurrentFormValid = valid;
  }

  save(): void {
    let command = this.mainForm.getRawValue();
    if (validateForm(this.mainForm)) {
      this.disableForm();
      command = trim(command);
      this.sub.sink = this.companyService.update(this.currentCompanyId, command).subscribe((result) => {
        if (result && result.ok) {
          this.translateService.get('COMPANY_EDIT.MESSAGES.SUCCESS').subscribe((t) => {
            this.toastr.success(t);
          });
        }
        this.enableForm();
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
        this.enableForm();
      });
    } else {
      this.translateService.get('COMPANY_EDIT.MESSAGES.INVALID_FORM').subscribe((t) => {
        this.toastr.error(t);
      });
    }
  }
}
