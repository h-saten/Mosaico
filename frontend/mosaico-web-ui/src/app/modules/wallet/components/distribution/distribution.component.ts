import { Component, Input, OnDestroy, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { Token, TokenDistribution, TokenDistributionService, TokenService, TOKEN_PERMISSIONS, UpsertTokenDistributionCommand } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectToken, setToken } from '../../store';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { selectTokenPermissions } from '../../store/wallet.selectors';
import { ProjectService, Stage, UpsertProjectStageCommand } from 'mosaico-project';
import { BehaviorSubject } from 'rxjs';
import { TokenDashboardService } from '../../services/token-dashboard.service';
import {v4 as uuidv4} from 'uuid';

@Component({
  selector: 'app-distribution',
  templateUrl: './distribution.component.html',
  styleUrls: ['./distribution.component.scss']
})
export class DistributionComponent extends FormBase implements OnInit, OnDestroy, OnChanges {
  distributionGroups: FormArray = new FormArray([]);
  subs = new SubSink();
  token: Token;
  isDataLoaded = false;
  isLoading$ = new BehaviorSubject<boolean>(false);
  canEdit = false;
  @Input() public service: TokenDashboardService;
  @Output() public refreshRequested = new EventEmitter<any>(null);

  constructor(private store: Store, private tokenService: TokenDistributionService, private tokService: TokenService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private translateService: TranslateService, private projectService: ProjectService) { super(); }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.service && !this.isDataLoaded){
      this.subs.sink = this.service?.onTokenDistributionUpdated.subscribe((ds) => {
        this.reset();
        if(ds){
          this.loadDistribution();
          this.isDataLoaded = true;
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
    this.subs.sink = this.store.select(selectToken).subscribe((res) => {
      this.token = res;
    });
    this.subs.sink = this.store.select(selectTokenPermissions).subscribe((res) => {
      this.canEdit = res && res[TOKEN_PERMISSIONS.CAN_EDIT] === true;
    });
    this.subs.sink = this.isLoading$.subscribe((val) => {
      if (val === true) {
        this.startLoading();
      }
      else {
        this.stopLoading();
      }
    });
  }

  startLoading(): void {
    this.form.disable();
  }

  stopLoading(): void {
    this.form.enable();
    this.disableAllPercentage();
  }

  loadDistribution(): void {
    const tokenDistributions = this.service?.tokenDistribution;
    if(tokenDistributions) {
      this.updateFormValues(tokenDistributions);
    }
  }

  updateFormValues(distributions: TokenDistribution[]): void {
    if (distributions && distributions.length > 0) {
      this.distributionGroups.clear();
      distributions.forEach((d) => {
        d['percent'] = 0;
        this.addGroup(true);
      });
      this.form.setValue({
        tokenDistributions: distributions?.map((d) => { return { ...d, totalValue: 0 }; })
      });
      distributions.forEach((d, i) => {
        if(d.blocked === true) {
          const array = this.form.get('tokenDistributions') as FormArray;
          if(array) {
            const control = array?.controls[i]?.get('tokenAmount');
            control?.disable();
          }
        }
      });
      this.recalculatePercentage();
    }
  }

  createForm(): void {
    this.form = new FormGroup({
      tokenDistributions: this.distributionGroups
    });
    this.disableAllPercentage();
    this.disableWholeFormIfNoPermissions();
  }

  disableWholeFormIfNoPermissions(): void {
    if (!this.canEdit) {
      this.form.disable();
    }
    else {
      this.form.enable();
      this.disableAllPercentage();
    }
  }

  recalculatePercentage(): void {
    this.distributionGroups?.controls?.forEach((c) => {
      const formGroup = c as FormGroup;
      if (formGroup) {
        const tokenAmount = +formGroup?.get('tokenAmount')?.value;
        if (tokenAmount > 0) {
          formGroup?.get('percent')?.setValue((+tokenAmount * 100 / this.token.totalSupply).toFixed(2));
        }
      }
    });
  }

  addGroup(exists = false): void {
    if (!this.isLoading$.value) {
      if (this.distributionGroups && this.distributionGroups.length >= 15) {
        this.showInvalidFormMessage('DISTRIBUTION.EXCEEDED_LIMIT');
        return;
      }
      const form = this.createDistributionGroup();
      this.initiatePercentageSubscription(form);
      this.distributionGroups.push(form);
      this.disableAllPercentage();
      this.disableWholeFormIfNoPermissions();
      if(!exists) {
        const id = uuidv4();
        form.get('id')?.setValue(id);
        const d: TokenDistribution = {
          id,
          name: null,
          tokenAmount: 0
        };
        this.service.addDistributionGroup(d);
      }
    }
  }

  private initiatePercentageSubscription(form: FormGroup): void {
    this.subs.sink = form?.get('tokenAmount').valueChanges.subscribe((v) => {
      if (this.token && this.token.totalSupply > 0) {
        const percentContorl = form?.get('percent');
        let newValue = 0;
        if (percentContorl) {
          if (v && +v > 0) {
            newValue = +v;
            percentContorl.setValue((+v * 100 / this.token.totalSupply).toFixed(2));
          }
          else {
            percentContorl.setValue(newValue);
          }
          const index = this.service.tokenDistribution?.findIndex((d) => d.id === form.get('id')?.value);
          if(index >= 0){
            this.service.updateTokenDistribution(index, newValue);
          }
        }
      }
    });
  }

  createDistributionGroup(): FormGroup {
    return new FormGroup({
      id: new FormControl(null),
      name: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(150)]),
      percent: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(100)]),
      tokenAmount: new FormControl(null, [Validators.required, Validators.min(0)]),
      tokenPrice: new FormControl(null),
      totalValue: new FormControl(null),
      projectId: new FormControl(null),
      blocked: new FormControl(false),
      balance: new FormControl(0)
    });
  }

  private disableAllPercentage(): void {
    if (this.distributionGroups) {
      this.distributionGroups.controls.forEach(gr => {
        gr.get('percent')?.disable();
        gr.get('tokenPrice')?.disable();
        gr.get('totalValue')?.disable();
      });
    }
  }

  deleteGroup(i: number): void {
    this.distributionGroups.removeAt(i);
    this.service.deleteGroup(i);
  }

  getGroupForm(i: number): FormGroup {
    return this.distributionGroups?.controls[i] as FormGroup;
  }

  public async saveDistribution(): Promise<void> {
    if (!this.isLoading$.value) {
      this.isLoading$.next(true);
      if (validateForm(this.form)) {
        const command = this.form.getRawValue() as UpsertTokenDistributionCommand;
        if (command) {
          if (this.service.isValid) {
            try {
              const result = await this.tokenService.upsert(this.token.id, command).toPromise();
              const t = await this.translateService.get('DISTRIBUTION.MESSAGES.SUCCESS').toPromise();
              this.toastr.success(t);
              this.isLoading$.next(false);
              this.refreshRequested.emit(true);
            }
            catch (error) {
              this.errorHandler.handleErrorWithToastr(error);
              this.isLoading$.next(false);
            }
          }
          else {
            this.showInvalidFormMessage('DISTRIBUTION.INVALID_FORM');
            this.isLoading$.next(false);
          }
        }
      }
      else {
        this.showInvalidFormMessage('DISTRIBUTION.INVALID_FORM');
        this.isLoading$.next(false);
      }
    }
  }

  showInvalidFormMessage(key: string): void {
    this.translateService.get(key).subscribe((res) => {
      this.toastr.error(res);
    });
  }

  reset(): void {
    this.distributionGroups.clear();
    this.createForm();
    this.isDataLoaded = false;
  }

  onVaultDeployed(res: boolean): void {
    if(res === true){
      this.subs.sink = this.tokService.getToken(this.token.id).subscribe((res) => {
        this.token = res?.data;
        this.store.dispatch(setToken({token: this.token}));
        this.refreshRequested.emit(true);
      });
    }
  }

  onDepositCreated(res: boolean): void {
    if(res === true) {
      this.refreshRequested.emit(true);
    }
  }

  onTokensSent(res: boolean): void {
    if(res === true) {
      this.refreshRequested.emit(true);
    }
  }
}
