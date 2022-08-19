import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FileParameters, FormBase, FormModeEnum, trim, validateForm } from 'mosaico-base';
import { CreateUpdateProjectPackageCommand, ProjectPackage, TokenPageService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { ImageCropperLocalComponent } from 'src/app/modules/shared-modules/image-cropper-local/image-cropper-local.component';
import { SubSink } from 'subsink';
import { ProjectPathEnum } from '../../../constants';
import { selectProjectPackages } from '../../../store';

@Component({
  selector: 'app-project-packages-form',
  templateUrl: './project-packages-form.component.html',
  styleUrls: ['./project-packages-form.component.scss']
})
export class ProjectPackagesFormComponent extends FormBase implements OnInit, OnDestroy, OnChanges {

  private subs: SubSink = new SubSink();

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  @Input() currentFormMode: FormModeEnum;

  @Input() packageId?: string;
  @Input() projectId: string;
  @Input() pageId: string;
  @Input() currentLang: string;

  @Output() saveEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() cancelEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  itemList: ProjectPackage[] = [];
  packageUrlLogo = '';

  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;

  @ViewChild(ImageCropperLocalComponent) imageCropperLocal: ImageCropperLocalComponent;

  // addingStarted = false;

  // necessary - they are used by the parent - modal, sometime for removal
  dataSavingRequestInProgress = false;

  isEditingIsInProgress = false;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private errorHandling: ErrorHandlingService,
    private toastr: ToastrService,
    private tokenPageService: TokenPageService,
    private translateService: TranslateService
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.currentLang?.firstChange === false) {
      this.getProjectPackages();
    }
  }

  ngOnInit(): void {

    this.createForm();

    if (this.currentFormMode === FormModeEnum.Edit) {
      this.getProjectPackages();
    } else {
      this.packageUrlLogo = '';
      // this.packageUrlLogo = '/assets/media/tokenpage/benefit-img-default-edit.svg';
    }
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      tokenAmount: ['', [Validators.required, Validators.min(1)]],
      logoUrl: [''],
      benefit1: ['', [Validators.minLength(3), Validators.maxLength(500)]],
      benefit2: ['', [Validators.minLength(3), Validators.maxLength(500)]],
      benefit3: ['', [Validators.minLength(3), Validators.maxLength(500)]],
      benefit4: ['', [Validators.minLength(3), Validators.maxLength(500)]],
      benefit5: ['', [Validators.minLength(3), Validators.maxLength(500)]],
    });
  }

  private updateFormValue(projectPackage: ProjectPackage | null | undefined): void {
    if (projectPackage) {
      this.form.patchValue({
        title: (projectPackage.name ? projectPackage.name : ''),
        tokenAmount: (projectPackage.tokenAmount ? projectPackage.tokenAmount : ''),
        logoUrl: [projectPackage.logoUrl ? projectPackage.logoUrl : '']
      });

      if (projectPackage.benefits) {
        this.form.patchValue({
          benefit1: (projectPackage.benefits[0] ? projectPackage.benefits[0] : ''),
          benefit2: (projectPackage.benefits[1] ? projectPackage.benefits[1] : ''),
          benefit3: (projectPackage.benefits[2] ? projectPackage.benefits[2] : ''),
          benefit4: (projectPackage.benefits[3] ? projectPackage.benefits[3] : ''),
          benefit5: (projectPackage.benefits[4] ? projectPackage.benefits[4] : '')
        });
      }
    }
  }

  private getProjectPackages(): void {
    if (this.packageId) {

      this.subs.sink = this.store.select(selectProjectPackages).subscribe(data => {
        if (data) {
          this.itemList = data;

          if (this.itemList) {
            const projectPackage = this.itemList.find((m) => m.id === this.packageId);
              if (projectPackage) {

                this.updateFormValue(projectPackage);

                this.packageUrlLogo = projectPackage.logoUrl;
                // this.packageUrlLogo = projectPackage.logoUrl ? projectPackage.logoUrl : '/assets/media/tokenpage/benefit-img-default-edit.svg';
              }
          }
        }
      });
    }
  }

  editingIsInProgress(event: boolean): void {
    // this.addingStarted = true;
    this.isEditingIsInProgress = event;
  }

  editingCancel(): void {
    // this.addingStarted = false;
  }

  save(): void {
    if (validateForm(this.form)) {

      this.dataSavingRequestInProgress = true;

      let command = this.form.getRawValue() as CreateUpdateProjectPackageCommand;
      command = trim(command);

      command.name = this.form.controls.title.value;

      const b1 = this.form.controls.benefit1.value;
      const b2 = this.form.controls.benefit2.value;
      const b3 = this.form.controls.benefit3.value;
      const b4 = this.form.controls.benefit4.value;
      const b5 = this.form.controls.benefit5.value;

      command.benefits = [ b1, b2, b3, b4, b5 ];
      command.language = this.currentLang;

      this.form.disable();

      if (this.pageId) {

        // edit
        if (this.packageId) {
          this.subs.sink = this.tokenPageService.updatePackage(this.pageId, this.packageId, command).subscribe((result) => {
              if (result) {

                this.saveLogo(this.packageId);
                this.toastr.success('Package details were successfully updated');
                // this.saveEvent.emit(true);
              }
            }, (error) => {
              this.errorHandling.handleErrorWithToastr(error);
            }, () => {
          });

        // add
        } else {
          this.subs.sink = this.tokenPageService.createPackage(this.pageId, command).subscribe((result) => {
              if (result && result.ok === true) {

                this.saveLogo(result.data);
                this.toastr.success(this.translateService.instant('PACKAGE_ADDED'));
                // this.saveEvent.emit(true);
              }
            }, (error) => {
              this.errorHandling.handleErrorWithToastr(error);
            }, () => {
          });
        }

      } else {
        this.toastr.error('Form has invalid values. Please fix issue to continue');
      }

    } else {
      this.toastr.error(this.translateService.instant('PACKAGE_invalid_form'));
    }
  }

  private saveLogo(packageId: string): void {
    if (packageId) {

      // let packageLogo: File | null = null;

      const fileParameters: FileParameters = this.imageCropperLocal.getFileParameters();

      if (fileParameters && fileParameters.removeImg === true) {
        this.subs.sink = this.tokenPageService.deletePackageLogo(this.pageId, packageId).subscribe((res) => {
            this.saveEvent.emit(true);
          },
          (error) => { this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = true;}
        );

      } else if (fileParameters && fileParameters.data) {
        this.subs.sink = this.tokenPageService.updatePackageLogo(this.pageId, packageId, fileParameters.data).subscribe((result) => {
            if (result) {
              this.toastr.success('Logo were successfully updated');
              this.saveEvent.emit(true);
            }
          }, (error) => {
            this.errorHandling.handleErrorWithToastr(error);
          }, () => {
          });

      } else {
        this.saveEvent.emit(true);
      }

    } else {
      this.saveEvent.emit(true);
    }

  }

  cancel(): void {
    this.form.reset();
    this.cancelEvent.emit(true);
  }

}
