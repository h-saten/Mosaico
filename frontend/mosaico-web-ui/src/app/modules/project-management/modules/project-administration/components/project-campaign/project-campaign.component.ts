import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import moment from 'moment';
import { ErrorHandlingService, FormBase, FormDialogBase, trim, validateForm } from 'mosaico-base';
import { ProjectService, Stage } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { selectProjectPreview, setCurrentProjectCaps, setProjectStages } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-project-campaign',
  templateUrl: './project-campaign.component.html',
  styleUrls: ['./project-campaign.component.scss']
})
export class ProjectCampaignComponent extends FormBase implements OnInit, OnDestroy {
  subs: SubSink = new SubSink();
  stagesControl: FormArray;
  updateTimer: NodeJS.Timer;
  projectId: string;

  constructor(private store: Store, private projectService: ProjectService, private translateService: TranslateService,
    private toastr: ToastrService, private errorHandler: ErrorHandlingService, private formBuilder: FormBuilder) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    if (this.updateTimer) {
      clearInterval(this.updateTimer);
    }
  }

  ngOnInit(): void {
    this.createForm();
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      if (project?.project?.stages) {
        this.projectId = project.project?.id;
        this.updateStagesControl(project.project.stages, project.project.hardCap, project.project.softCap);
      }
    });
  }

  private updateStagesControl(stages: Stage[], hardCap: number, softCap: number): void {
    this.stagesControl.clear();
    this.form.get('softCap').setValue(softCap);
    this.form.get('hardCap').setValue(hardCap);
    stages.forEach((stage) => {
      const form = this.getStageForm();
      form.setValue({
        id: stage.id,
        name: stage.name,
        isPrivate: stage.type === 'Private',
        startDate: moment(stage.startDate).format('YYYY-MM-DDTHH:mm'),
        endDate: moment(stage.endDate).format('YYYY-MM-DDTHH:mm'),
        tokensSupply: stage.tokensSupply,
        tokenPrice: stage.tokenPrice,
        minimumPurchase: stage.minimumPurchase,
        maximumPurchase: stage.maximumPurchase
      });
      this.stagesControl.push(form);
    });
  }

  private createForm(): void {
    this.stagesControl = new FormArray([]);
    this.form = this.formBuilder.group({
      hardCap: new FormControl(null, [Validators.required, Validators.min(0)]),
      softCap: new FormControl(null, [Validators.required, Validators.min(0)]),
      stages: this.stagesControl
    });
    this.stagesControl.push(this.getStageForm());
    this.updateTimer = setInterval(() => {
      if (this.form && this.form.touched) {
        validateForm(this.form);
      }
    }, 500);
    this.form.get('softCap').disable();
    this.form.get('hardCap').disable();
    // this.subs.sink = this.form.get('hardCap').valueChanges.subscribe((newVal: number) => {
    //   if(newVal > 0 && !isNaN(+newVal)) {
    //     this.form.get('softCap').setValue(Math.round(newVal * 0.3));
    //   }
    // });
  }

  save(): void {
    if (validateForm(this.form)) {
      let command = this.form.getRawValue();
      this.disableForm();
      command = trim(command);
      this.subs.sink = this.projectService.updateStages(this.projectId, command).subscribe((result) => {
        if (result && result.data) {
          this.translateService.get('MODALS.CAMPAIGN_EDITOR.MESSAGES.UPDATE_SUCCESS').subscribe((translation) => {
            this.toastr.success(translation);
          });
          this.store.dispatch(setProjectStages({ stages: result.data }));
          this.store.dispatch(setCurrentProjectCaps({ hardCap: command.hardCap, softCap: command.softCap}));
        }
        this.enableForm();
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.enableForm(); });
    }
    else {
      this.subs.sink = this.translateService.get('MODALS.CAMPAIGN_EDITOR.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastr.error(res);
      });
    }
  }

  private getStageForm(): FormGroup {
    return this.formBuilder.group({
      id: [],
      name: ['New stage', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      isPrivate: [false],
      startDate: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      endDate: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]],
      tokensSupply: ['', [Validators.required, Validators.min(1)]],
      tokenPrice: ['', [Validators.required, Validators.min(0.01)]],
      minimumPurchase: ['', [Validators.required, Validators.min(0)]],
      maximumPurchase: ['']
    });
  }

  addStage(): void {
    const stageForm = this.getStageForm();
    this.stagesControl.push(stageForm);
  }

  deleteStage(f: FormGroup, index: number): void {
    this.stagesControl.removeAt(index);
  }

  getStageControls(f: FormGroup): FormGroup[] {
    const stagesControl = f.controls.stages as FormArray;
    return stagesControl.controls.map((a) => a as FormGroup);
  }

  enableForm(): void {
    this.form.enable();
    this.form.get('softCap').disable();
  }

  disableForm(): void {
    this.form.disable();
  }

}
