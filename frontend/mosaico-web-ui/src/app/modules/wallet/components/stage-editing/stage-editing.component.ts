import { Component, Input, OnDestroy, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormArray, FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { Stage, ProjectService } from 'mosaico-project';
import { Token, TokenDistribution, TokenDistributionService, TOKEN_PERMISSIONS, UpsertTokenDistributionCommand } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';
import { TokenDashboardService } from '../../services/token-dashboard.service';
import { setProjectStages } from 'src/app/modules/project-management/store';
import { selectToken, selectTokenPermissions } from '../../store';
import {v4 as uuidv4} from 'uuid';

@Component({
  selector: 'app-stage-editing',
  templateUrl: './stage-editing.component.html',
  styleUrls: ['./stage-editing.component.scss']
})
export class StageEditingComponent extends FormBase implements OnInit, OnDestroy, OnChanges {
  projectStagesControls: { [key: string]: FormArray } = {};
  subs = new SubSink();
  token: Token;
  stagesLoaded = false;
  isLoading$ = new BehaviorSubject<boolean>(false);
  canEdit = false;
  @Input() public service: TokenDashboardService;
  @Output() public refreshRequested = new EventEmitter<any>(null);

  constructor(private store: Store, private tokenService: TokenDistributionService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService, private translateService: TranslateService, private projectService: ProjectService) { super(); }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.service && !this.stagesLoaded){
      this.subs.sink = this.service?.onStagesUpdated.subscribe((ds) => {
        this.reset();
        if(ds){
          this.loadStages();
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
    if (this.token?.projects) {
      for (var i = 0; i < this.token.projects?.length; i++) {
        this.projectStagesControls[this.token.projects[i].id]?.disable();
      }
    }
  }

  stopLoading(): void {
    this.form.enable();
    if (this.token?.projects) {
      for (var i = 0; i < this.token.projects?.length; i++) {
        this.projectStagesControls[this.token.projects[i].id]?.enable();
      }
    }
  }

  loadStages(): void {
    this.projectStagesControls = {};
    const stages = this.service?.stages;
    if (stages) {
      Object.keys(stages)?.forEach((s) => {
        const projectStages = stages[s];
        if(projectStages) {
          projectStages?.forEach((projectStage) => {
            this.addStage(s, projectStage);
          });
        }
      });
    }
    this.stagesLoaded = true;
  }

  createForm(): void {
    this.form = new FormGroup({});
    this.disableWholeFormIfNoPermissions();
  }

  disableWholeFormIfNoPermissions(): void {
    if (!this.canEdit) {
      this.form.disable();
    }
    else {
      this.form.enable();
    }
  }

  private initiatePercentageSubscription(form: FormGroup): void {
    form?.get('tokenAmount').valueChanges.subscribe((v) => {
      if (this.token && this.token.totalSupply > 0) {
        const percentContorl = form?.get('percent');
        if (percentContorl) {
          let newValue = 0;
          if (v && +v > 0) {
            newValue = +v;
            percentContorl.setValue((+v * 100 / this.token.totalSupply).toFixed(2));
          }
          else {
            percentContorl.setValue(0);
          }
          const id = form?.get('id')?.value;
          const projectId = form?.get('projectId')?.value;
          const index = this.service.stages[projectId]?.findIndex((st) => st.id && st.id === id);
          if(index >= 0){
            this.service.updateStage(projectId, index, newValue);
          }
        }
      }
    });
  }

  public async saveStages(projectId: string): Promise<void> {
    if (!this.isLoading$.value) {
      this.isLoading$.next(true);
      try {
        const stages = this.service.stages[projectId];
        stages?.forEach((s, j) => {
          const form = this.projectStagesControls[projectId].controls[j];
          s.name = form.get('name')?.value;
          s.tokenPrice = form.get('tokenPrice')?.value;
          s.tokensSupply = form.get('tokenAmount')?.value;
        });
        const result = await this.projectService.updateStages(projectId, { stages }).toPromise();
        const t = await this.translateService.get('DISTRIBUTION.MESSAGES.STAGE_SUCCESS').toPromise();
        this.store.dispatch(setProjectStages({ stages: stages }));
        this.toastr.success(t);
        this.refreshRequested.emit(true);
        this.isLoading$.next(false);
      }
      catch (error) {
        this.errorHandler.handleErrorWithToastr(this.translateService.instant('DISTRIBUTION.MESSAGES.STAGE_FAILED'));
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
    this.projectStagesControls = {};
    this.createForm();
    this.stagesLoaded = false;
    this.loadStages();
  }


  addStage(id: string, s: Stage): void {
    if (!this.projectStagesControls[id]) {
      this.projectStagesControls[id] = new FormArray([]);
    }

    if (!this.service.stages[id]) {
      this.service.stages[id] = [];
    }

    if(!s.id) {
      s.id = uuidv4();
      s.projectId = id;
    }

    const existingStageIndex = this.service.stages[id].findIndex((st) => st.id && st.id === s.id);
    if (existingStageIndex >= 0) {
      this.service.stages[id][existingStageIndex] = s;
      let form = this.projectStagesControls[id].controls[existingStageIndex] as FormGroup;
      if(!form) {
        form = this.createControl();
        this.projectStagesControls[id].controls[existingStageIndex] = form;
      }
      this.setFormValues(form, s);
    }
    else {
      const form = this.createControl();
      this.projectStagesControls[id].push(form);
      this.service.stages[id].push(s);
      this.setFormValues(form, s);
    }
  }

  private setFormValues(form: FormGroup, s: Stage): void {
    form.get('id').setValue(s?.id);
    form.get('name').setValue(s?.name);
    form.get('tokenPrice').setValue(s?.tokenPrice);
    form.get('tokenAmount').setValue(s?.tokensSupply);
    form.get('projectId').setValue(s?.projectId);
  }

  private createControl(): FormGroup {
    const form = this.createDistributionGroup();
    this.subs.sink = form.get('tokenPrice')?.valueChanges.subscribe((newTokenPrice) => {
      this.calculateValue(form);
    });
    this.subs.sink = form.get('tokenAmount')?.valueChanges.subscribe((newTokenPrice) => {
      this.calculateValue(form);
    });
    this.initiatePercentageSubscription(form);
    this.disableWholeFormIfNoPermissions();
    return form;
  }

  private createDistributionGroup(): FormGroup {
    return new FormGroup({
      id: new FormControl(null),
      name: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(150)]),
      percent: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(100)]),
      tokenAmount: new FormControl(null, [Validators.required, Validators.min(0)]),
      tokenPrice: new FormControl(null),
      totalValue: new FormControl(null),
      projectId: new FormControl(null)
    });
  }

  private calculateValue(form: FormGroup): void {
    const tokenAmount = form.get('tokenAmount')?.value;
    const tokenPrice = form.get('tokenPrice')?.value;
    form.get('totalValue')?.setValue(tokenAmount * tokenPrice);
  }

  getStageForm(id: string, i: number): FormGroup {
    return this.projectStagesControls[id]?.controls[i] as FormGroup;
  }

  deleteStage(id: string, i: number): void {
    this.service.deleteStage(id, i);
    this.projectStagesControls[id]?.removeAt(i);
  }

}
