import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { Partner, ProjectService } from 'mosaico-project';
import { ImageCroppedEvent, LoadedImage } from 'ngx-image-cropper';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectProjectPreview } from '../../../store';

@Component({
  selector: 'app-project-partner-add-edit',
  templateUrl: './project-partner-add-edit.component.html',
  styleUrls: ['./project-partner-add-edit.component.scss']
})
export class ProjectPartnerAddEditComponent extends FormBase implements OnInit {

  @Input() projectId: string;
  partnerPhoto: string;
  cropImage: boolean = false;
  pageId: string;
  imageChangedEvent: any = '';
  croppedImage: any;
  private subs: SubSink = new SubSink();
  croppedFileObject: File;
  partnerImage: any = null;
  imagePicker: any = null;
  partnerProfile: string = '';
  partner: Partner[] = [];

  selectedPartner: any = null;


  constructor(
    public activeModal: NgbActiveModal,
    private store: Store,
    private formBuilder: FormBuilder,
    private toastrService: ToastrService,
    private translateService: TranslateService,
    private projectService: ProjectService,
    private errorHandling: ErrorHandlingService) {
    super();
    const reg = /^(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})$/;

    this.form = this.formBuilder.group({
      id: new FormControl('', []),
      name: new FormControl('', [Validators.required]),
      position: new FormControl('', [Validators.required]),
      facebook: new FormControl('', [Validators.pattern(reg)]),
      twitter: new FormControl('', [Validators.pattern(reg)]),
      linkedIn: new FormControl('', [Validators.pattern(reg)]),
      order: new FormControl('', []),
      photoUrl: new FormControl('', [])
    });
  }

  get m() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.partnerPhoto = this.form.controls.photoUrl.value;
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.pageId = data.project?.pageId;
      }
    });
    this.partnerProfile = '';
    if (this.projectId) {
      this.form.controls.projectId?.setValue(this.projectId);
    }
    if (this.partnerPhoto) {
      this.partnerImage = this.partnerPhoto;
    }
    else {
      this.partnerImage = "";
    }

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.projectId) {
      this.ngOnInit();
    }
  }

  showMenu(member: any, index: number): void {

  }

  savePartner() {
    if (validateForm(this.form)) {
      let partnerRecord: Partner;
      partnerRecord = {
        name: this.form.value.name,
        position: this.form.value.position,
        facebook: this.form.value.facebook,
        linkedIn: this.form.value.linkedIn,
        twitter: this.form.value.twitter,
        pageId: this.pageId,
        order: 0,
        photoUrl: this.form.value.photoUrl,
        profileUrl: this.form.value.profileUrl,
      }
      if (this.form.controls.id.value) {
        partnerRecord.id = this.form.value.id
      }
      if (this.form.controls.order.value) {
        partnerRecord.order = this.form.value.order
      }

      let isWithProfile: boolean = this.croppedImage ? true : false;
      this.addUpdateProjectPartner(partnerRecord, isWithProfile);
    } else { 
        this.toastrService.error(this.translateService.instant('PACKAGE_invalid_form')); 
    } 
  }

  addUpdateProjectPartner(partnerRecord: Partner, isWithProfile: boolean = false): void {
    if (isWithProfile) {
      this.projectService.uploadPartnerProfile(this.projectId, this.croppedFileObject).subscribe(resFile => {
        partnerRecord.profileUrl = resFile.data;
        partnerRecord.photoUrl = resFile.data;
        this.projectService.addUpdatePartner(partnerRecord).subscribe((res: any) => {
          if (res.data) {
            this.translateService.get('PROJECT_PARTNER.SAVED_SUCCESSFULLY').subscribe((t) => {
              this.toastrService.success(t);
            });
            this.activeModal.close();
            this.ngOnInit();
          }
          else {
            this.toastrService.error(res?.message);
          }
        }, (error) => { 
          this.errorHandling.handleErrorWithToastr(error); 
        });
      }, (error) => { 
        this.errorHandling.handleErrorWithToastr(error); 
      });
    } else {
      this.projectService.addUpdatePartner(partnerRecord).subscribe(res => {
        if (res.data) {
          this.translateService.get('PROJECT_PARTNER.SAVED_SUCCESSFULLY').subscribe((t) => {
            this.toastrService.success(t);
          });
          this.activeModal.close();
          this.ngOnInit();
        }
      });
    }
    this.partnerProfile = '';
  }

  toggleCropper(): void {
    if (this.cropImage && this.croppedImage) {
      //Handle Cropped Image here.
      this.partnerImage = this.croppedImage;
    }
    this.cropImage = !this.cropImage;
  }

  imageCropped(event: ImageCroppedEvent): void {
    this.croppedImage = event.base64;
    this.partnerImage = event.base64;
    this.croppedFileObject = this.base64ToFile(
      this.croppedImage,
      this.imageChangedEvent.target.files[0].name,
    );
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
    this.cropImage = true;
  }

  removeImage(): void {
    this.croppedImage = null;
    this.imageChangedEvent = '';
    this.partnerImage = null;
    this.imagePicker = null;
  }

  cancelCrop(): void {
    this.cropImage = false;
    this.croppedImage = null;
    this.partnerImage = null;
  }

  base64ToFile(data: any, filename: string): File {
    const arr = data.split(',');
    const mime = arr[0].match(/:(.*?);/)[1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    let u8arr = new Uint8Array(n);
    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], filename, { type: mime });
  }

  imageLoaded(image: LoadedImage) {
  }

  cropperReady() {
  }

  loadImageFailed() {
  }
  
}
