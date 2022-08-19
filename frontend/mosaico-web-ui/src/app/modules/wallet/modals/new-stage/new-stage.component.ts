import { Component, OnInit, OnDestroy, Output, EventEmitter, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorHandlingService, FormDialogBase, trim, validateForm } from 'mosaico-base';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { ProjectService, Stage, UpsertProjectStageCommand } from 'mosaico-project';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';
import moment from 'moment';

@Component({
  selector: 'app-new-stage',
  templateUrl: './new-stage.component.html',
  styleUrls: ['./new-stage.component.scss']
})
export class NewStageComponent extends FormDialogBase implements OnInit, OnDestroy {
  subs = new SubSink();
  @Input() id: string;
  stage?: Stage;
  @Output() onCreated = new EventEmitter<Stage>();
  isLoading$ = new BehaviorSubject<boolean>(false);
  isEdit = false;

  constructor(modalService: NgbModal, private toastr: ToastrService, private projectService: ProjectService, private translateService: TranslateService,
    private errorHandlingService: ErrorHandlingService) { super(modalService); }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  open(stage?: Stage): void {
    this.stage = stage;
    this.ngOnInit();
    if(this.stage) {
      this.isEdit = true;
      this.fillForm();
    }
    super.open();
  }

  private fillForm(): void {
    if(this.stage) {
      this.form.setValue({
        name: this.stage.name,
        isPrivate: this.stage.type === 'Private',
        startDate: new Date(this.stage.startDate),
        endDate: new Date(this.stage.endDate),
        minimumPurchase: this.stage.minimumPurchase,
        maximumPurchase: this.stage.maximumPurchase
      });
    }
  }

  ngOnInit(): void {
    this.createForm();
    this.subs.sink = this.isLoading$.subscribe((val) => {
      if(val === true) {
        this.disableForm();
      }
      else {
        this.enableForm();
      }
    });
  }

  private createForm(): void {
    this.form = new FormGroup({
      name: new FormControl(null, [Validators.required, Validators.minLength(1), Validators.maxLength(30)]),
      isPrivate: new FormControl(false),
      startDate: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
      endDate: new FormControl(null, [Validators.required, Validators.minLength(1), Validators.maxLength(50)]),
      minimumPurchase: new FormControl(null, [Validators.required, Validators.min(0)]),
      maximumPurchase:  new FormControl(null)
    });
  }

  save(): void {
    if (validateForm(this.form)) {
      let command = this.isEdit ? {...this.stage, ...this.form.getRawValue()} as UpsertProjectStageCommand : this.form.getRawValue() as UpsertProjectStageCommand;
      if(this.form.getRawValue().isPrivate === true){
        command.type = 'Private';
      }
      else {
        command.type = 'Public';
      }

      this.isLoading$.next(true);
      command = trim(command);
      this.onCreated.emit({
        endDate: command.endDate,
        id: command.id,
        type: command.type,
        maximumPurchase: command.maximumPurchase,
        minimumPurchase: command.minimumPurchase,
        name: command.name,
        startDate: command.startDate,
        hardcap: this.stage?.hardcap,
        softcap: this.stage?.softcap,
        tokenPrice: this.stage?.tokenPrice,
        tokensSupply: this.stage?.tokensSupply,
      });
      this.modalRef.close(command);
      this.isLoading$.next(false);
    }
    else {
      this.subs.sink = this.translateService.get('MODALS.CAMPAIGN_EDITOR.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastr.error(res);
      });
    }
  }

  enableForm(): void {
    this.form.enable();
  }

  disableForm(): void {
    this.form.disable();
  }

  // private updateStage(command): void {
  //   this.subs.sink = this.projectService.updateStage(this.id, command.id, command).subscribe((result) => {
  //     if (result && result.data) {
  //       this.translateService.get('MODALS.CAMPAIGN_EDITOR.MESSAGES.UPDATE_SUCCESS').subscribe((translation) => {
  //         this.toastr.success(translation);
  //       });
  //     }
  //     this.isLoading$.next(false);
  //     this.onCreated.emit({
  //       endDate: command.endDate,
  //       id: result.data,
  //       type: command.type,
  //       maximumPurchase: command.maximumPurchase,
  //       minimumPurchase: command.minimumPurchase,
  //       name: command.name,
  //       startDate: command.startDate,
  //       hardcap: 0,
  //       softcap: 0,
  //       tokenPrice: 0,
  //       tokensSupply: 0,
  //     });
  //     this.modalRef.close(command);
  //   }, (error) => { this.errorHandlingService.handleErrorWithToastr(error); this.isLoading$.next(false); });
  // }

  // private createStage(command): void {
  //   this.subs.sink = this.projectService.createStage(this.id, command).subscribe((result) => {
  //     if (result && result.data) {
  //       this.translateService.get('MODALS.CAMPAIGN_EDITOR.MESSAGES.UPDATE_SUCCESS').subscribe((translation) => {
  //         this.toastr.success(translation);
  //       });
  //     }
  //     this.isLoading$.next(false);
  //     this.onCreated.emit({
  //       endDate: command.endDate,
  //       id: result.data,
  //       type: command.type,
  //       maximumPurchase: command.maximumPurchase,
  //       minimumPurchase: command.minimumPurchase,
  //       name: command.name,
  //       startDate: command.startDate,
  //       hardcap: 0,
  //       softcap: 0,
  //       tokenPrice: 0,
  //       tokensSupply: 0,
  //     });
  //     this.modalRef.close(command);
  //   }, (error) => { this.errorHandlingService.handleErrorWithToastr(error); this.isLoading$.next(false); });
  // }
}
