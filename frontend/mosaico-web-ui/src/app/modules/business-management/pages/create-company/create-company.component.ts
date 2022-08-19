import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Country, CountryService, ErrorHandlingService, FormBase, trim, validateForm } from 'mosaico-base';
import { CompanyService, CreateCompanyCommand } from 'mosaico-dao';
import { ActiveBlockchainService, Blockchain, CompanyCreated, DaoCreationHubService, DeploymentEstimate, DeploymentEstimateHubService, SystemWallet, SystemWalletService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { BehaviorSubject } from 'rxjs';
import { selectUserInformation } from '../../../user-management/store/user.selectors';

@Component({
  selector: 'app-create-company',
  templateUrl: './create-company.component.html',
  styleUrls: ['./create-company.component.scss']
})
export class CreateCompanyComponent extends FormBase implements OnInit, OnDestroy {

  isCurrentFormValid = false;
  private sub: SubSink = new SubSink();
  countries: Country[] = [];
  isLoading$ = new BehaviorSubject<boolean>(false);
  isInitialLoading = true;
  networks: Blockchain[] = [];
  currentChainId: string;
  networkControl = new FormControl(null, [Validators.required]);
  userId: string;

  sizeOptions = [
    { value: 'S', key: 'COMPANY_EDIT.FORM.SIZE.OPTIONS.SMALL' },
    { value: 'M', key: 'COMPANY_EDIT.FORM.SIZE.OPTIONS.MEDIUM' },
    { value: 'L', key: 'COMPANY_EDIT.FORM.SIZE.OPTIONS.LARGE' }
  ];

  votingPeriods = [
    { value: 'Day', key: 'COMPANY_EDIT.FORM.PERIOD.OPTIONS.DAY' },
    { value: 'Week', key: 'COMPANY_EDIT.FORM.PERIOD.OPTIONS.WEEK' },
    { value: 'Month', key: 'COMPANY_EDIT.FORM.PERIOD.OPTIONS.MONTH' }
  ];

  createdCompanyId: string;
  deployedCompanyId: string;

  constructor(private formBuilder: FormBuilder, private companyService: CompanyService, private hub: DaoCreationHubService,
    private store: Store, private toastr: ToastrService, private errorHandling: ErrorHandlingService,
    private translateService: TranslateService, private router: Router, private countryService: CountryService, ) {
    super();
  }

  ngOnInit(): void {
    this.isInitialLoading = true;
    this.sub.sink = this.store.select(selectUserInformation).subscribe((u) => {
      this.userId = u?.id;
    });
    this.sub.sink = this.countryService.getCountries().subscribe((res) => {
      this.countries = res?.data;
      this.isInitialLoading = false;
    });
    this.sub.sink = this.store.select(selectCurrentActiveBlockchains).pipe(take(1)).subscribe((res) => {
      this.networks = res;
      if (this.networks) {
        this.createForm(this.networks[0]?.name);
      }
    });
    this.sub.sink = this.isLoading$.subscribe((v) => {
      if (v === true) {
        this.form.disable();
      }
      else {
        this.form.enable();
        this.disableFields();
      }
    });
    this.hub.startConnection();
    this.hub.addListener();
    this.sub.sink = this.hub.created$.subscribe((data: CompanyCreated) => {
      if (data) {
        this.deployedCompanyId = data.companyId;
        this.verifyPossibilityToRedirect(data.slug);
      }
    });
    this.sub.sink = this.hub.failed$.subscribe((error) => {
      if(error && error.length > 0) {
        this.toastr.error(error);
        this.isLoading$.next(false);
      }
    });
  }

  verifyPossibilityToRedirect(slug: string): void {
    if (this.deployedCompanyId && this.createdCompanyId && this.deployedCompanyId === this.createdCompanyId) {
      this.translateService.get('COMPANY_EDIT.MESSAGES.SUCCESS').subscribe((t) => {
        this.toastr.success(t);
      });
      this.router.navigateByUrl(`/dao/${slug}`);
    }
  }

  private createForm(networkName: string): void {
    this.form = this.formBuilder.group({
      companyName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      country: [null],
      street: [''],
      postalCode: [''],
      vatid: [''],
      size: [null],
      region: [''],
      network: this.networkControl,
      isVotingEnabled: [true],
      onlyOwnerProposals: [true],
      regulationAccepted: [false, [Validators.requiredTrue]],
      quorum: [20, [Validators.required, Validators.min(0), Validators.max(100)]],
      initialVotingDelay: ['Day', [Validators.required]],
      initialVotingPeriod: ['Week', [Validators.required]],
      wallet: ['MOSAICO_WALLET', [Validators.required]]
    });
    this.networkControl.setValue(networkName);
    this.disableFields();
  }


  disableFields(): void {
    this.form.get('quorum').disable();
    this.form.get('initialVotingDelay').disable();
    this.form.get('initialVotingPeriod').disable();
  }

  ngOnDestroy(): void {
    this.hub.removeListener();
    this.sub.unsubscribe();
    this.hub.resetObjects();
  }

  setCurrentFormValidated(valid: boolean): void {
    this.isCurrentFormValid = valid;
  }

  save(): void {
    let command = this.form.getRawValue() as CreateCompanyCommand;
    if (validateForm(this.form)) {
      this.isLoading$.next(true);
      command = trim(command);
      this.sub.sink = this.companyService.createCompany(command).subscribe((result) => {
        this.translateService.get('COMPANY_EDIT.MESSAGES.INITIATED_TRANSACTION').subscribe((t) => {
          this.toastr.success(t);
        });
        if (result && result.data) {
          this.createdCompanyId = result.data.companyId;
        }
        else {
          this.isLoading$.next(false);
        }
      }, (error) => {
        this.errorHandling.handleErrorWithToastr(error);
        this.isLoading$.next(false);
        this.form.enable();
      });
    } else {
      this.translateService.get('COMPANY_EDIT.MESSAGES.INVALID_FORM').subscribe((t) => {
        this.toastr.error(t);
      });
    }
  }

}
