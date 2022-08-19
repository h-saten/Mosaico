import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { LoadedImage, ImageCroppedEvent } from 'ngx-image-cropper';
import { ProjectService, TeamMember } from 'mosaico-project';
import { SubSink } from 'subsink';
import { Store } from '@ngrx/store';
import { selectProjectPreview } from '../../../store';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlingService, FormBase, validateForm } from 'mosaico-base';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-project-team-add-edit',
  templateUrl: './project-team-add-edit.component.html',
  styleUrls: ['./project-team-add-edit.component.scss']
})
export class ProjectTeamsAddEditComponent extends FormBase implements OnInit, OnChanges {
  @Input() projectId: string;
  memberPhoto: string;
  cropImage: boolean = false;
  pageId: string;
  imageChangedEvent: any = '';
  croppedImage: any;
  private subs: SubSink = new SubSink();
  croppedFileObject: File;
  memberImage: any = null;
  imagePicker: any = null;
  memberProfile: string = '';
  team: TeamMember[] = [];
  selectedMember: any = null;

  constructor(
    public activeModal: NgbActiveModal,
    private store: Store,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private projectService: ProjectService,
    private errorHandling: ErrorHandlingService,
    private translateService: TranslateService) {
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
      photoUrl: new FormControl('', []),
      pageId: new FormControl(null)
    });
  }

  get m() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.memberPhoto = this.form.controls.photoUrl.value;
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.pageId = data.project?.pageId;
      }
    });
    this.memberProfile = '';
    if (this.projectId) {
      this.form.controls.projectId?.setValue(this.projectId);
    }
    if (this.memberPhoto) {
      this.memberImage = this.memberPhoto;
    }
    else {
      this.memberImage = "";
    }

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.projectId) {
      this.ngOnInit();
    }
  }

  showMenu(member: any, index: number): void {

  }

  saveMember(): void { 
 
       
    if (validateForm(this.form)) { 
        let memberRecord: TeamMember; 
        memberRecord = { 
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
          memberRecord.id = this.form.value.id; 
        } 
        if (this.form.controls.order.value) { 
          memberRecord.order = this.form.value.order; 
        } 
         
        let isWithProfile: boolean = this.croppedImage ? true : false; 
 
        this.addUpdateProjectTeam(memberRecord, isWithProfile); 
      } else { 
        this.toastr.error(this.translateService.instant('PACKAGE_invalid_form')); 
      } 
     
  }
    

  addUpdateProjectTeam(memberRecord: TeamMember, isWithProfile: boolean = false) { 
    if (isWithProfile) { 
      this.projectService.uploadTeamMemberProfile(this.projectId, this.croppedFileObject).subscribe(resFile => { 
        memberRecord.profileUrl = resFile.data; 
        memberRecord.photoUrl = resFile.data; 
        this.projectService.addUpdateTeamMember(memberRecord).subscribe((res: any) => { 
          if (res.data) { 
            this.toastr.success('Member was saved'); 
            this.activeModal.close(true); 
            this.ngOnInit(); 
          } 
        }, (error) => { 
          this.errorHandling.handleErrorWithToastr(error); 
        } 
        ); 
      }, (error) => { 
        this.errorHandling.handleErrorWithToastr(error); 
      }  
      ); 
    } else { 
      memberRecord.photoUrl = ''; 
      this.projectService.addUpdateTeamMember(memberRecord).subscribe(res => { 
        if (res.data) { 
          this.toastr.success('Member was saved'); 
          this.activeModal.close(true); 
          this.ngOnInit(); 
        } 
      }, (error) => { 
        this.errorHandling.handleErrorWithToastr(error); 
      }  
      ); 
    } 
    this.memberProfile = ''; 
  }

  toggleCropper() {
    if (this.cropImage && this.croppedImage) {
      //Handle Cropped Image here.
      this.memberImage = this.croppedImage;
    }
    this.cropImage = !this.cropImage;
  }

  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
    this.memberImage = event.base64;
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
    this.memberImage = null;
    this.imagePicker = null;
  }

  cancelCrop(): void {
    this.cropImage = false;
    this.croppedImage = null;
    this.memberImage = null;
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
