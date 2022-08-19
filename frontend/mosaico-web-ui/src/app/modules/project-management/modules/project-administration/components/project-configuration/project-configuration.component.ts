import { Component, NgModuleRef, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { selectProjectPage } from '../../../../store/project.selectors';
import { PageIntroVideoComponent } from '../../../../modals';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IntroVideo } from 'mosaico-project';
import { TranslateService } from '@ngx-translate/core';
import { Page, TokenPageService } from 'mosaico-project';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FormBase, validateForm} from 'mosaico-base';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-project-configuration',
  templateUrl: './project-configuration.component.html',
  styleUrls: ['./project-configuration.component.scss']
})
export class ProjectConfigurationComponent extends FormBase implements OnInit {
  pageId: string;
  uploadedVideoFile: File;
  // IsExternalLink: boolean = false;
  // ShowLocalVideo: boolean = true;
  // VideoUrl: string;
  pageIntroVideos: IntroVideo;
  mainForm: FormGroup;
  public page: Page;
  private subs = new SubSink();
  private reg = /^(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})$/;
  constructor(
    private store: Store,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private pageService: TokenPageService,
    private errorHandling: ErrorHandlingService,
    private tokenPageService: TokenPageService
  ) { 
    super()
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res) {
        this.page = res
        this.getPageIntroVideos(this.page.id);
      }
    });
    this.createForm();
  }

  private createForm(): void{
    const videoLinkValidator = [
      Validators.pattern(this.reg),
      Validators.required
    ];
    const videoFileValidator = [
      Validators.required
    ];
    this.mainForm = this.formBuilder.group({
        videoUrl:[''],
        videoExternalLink:new FormControl(['']),
        videoFile:new FormControl(['']),
        showLocalVideo:new FormControl([''])
    });
    // this.mainForm.get('showLocalVideo').valueChanges.subscribe(value => {
    //   if(value){
    //     this.mainForm.get('videoFile').setValidators(videoFileValidator);
    //   }
    //   else
    //   {
    //     this.mainForm.get('videoExternalLink').setValidators(videoLinkValidator);
    //   }
    // });
  }

  private updateValue():void{
    
    if(this.pageIntroVideos)
    {
      this.mainForm.setValue({
        videoUrl: this.pageIntroVideos.videoUrl,
        videoExternalLink: this.pageIntroVideos.videoExternalLink,
        showLocalVideo:this.pageIntroVideos.showLocalVideo,
        videoFile:''
      });
    }
    else{
      this.mainForm.setValue({
        videoUrl:'',
        videoFile:'',
        videoExternalLink:'',
        showLocalVideo:false
      });
    }
  }
  get m(){
    return this.mainForm.controls;
  }
  private getPageIntroVideos(id: string): void{
    this.subs.sink = this.tokenPageService.getIntroVideos(id).subscribe((res) => {
      if(res && res.data){
        this.pageIntroVideos = res.data.introVideo;
        this.updateValue();
      }
    });
  }

  getFile(e) {

    let extensionAllowed = {"mp4":true};
    if (e.target.files[0].size / 1024 / 1024 > 100) {
      this.subs.sink = this.translateService.get('PAGE_INTRO.MESSAGE.FILE_SIZE').subscribe((res) => {
        this.toastr.error(res);
      });
      return;
    }
    if (extensionAllowed) {
      var nam = e.target.files[0].name.split('.').pop();
      if (!extensionAllowed[nam]) {
        this.subs.sink = this.translateService.get('PAGE_INTRO.MESSAGE.FILE_EXTENTION').subscribe((res) => {
          this.toastr.error(res);
        });
        return;
      }
    }
    this.uploadedVideoFile = e.target.files[0];
  }

  validateSubmitedData(): boolean{
    var videoExternalLink = this.mainForm.get("videoExternalLink").value;
    var isShowLocalVideo = this.mainForm.get("showLocalVideo").value;
    if(videoExternalLink || this.uploadedVideoFile)
    {
      if(!isShowLocalVideo && !videoExternalLink.match(this.reg))
      {
        this.subs.sink = this.translateService.get('PAGE_INTRO.MESSAGE.VIDEO_LINK').subscribe((res) => {
          this.toastr.success(res);
        });
        return false;
      }
      return true;
    }
    else{
      return false;
    }
  }

  save(): void {
    if (this.validateSubmitedData()) {
      if(this.page.id)
      {
        var isShowLocalVideo = this.mainForm.get("showLocalVideo").value;
        var videoExternalLink = this.mainForm.get("videoExternalLink").value;
        if(this.uploadedVideoFile){
          this.subs.sink = this.tokenPageService.uploadPageIntroVideo(this.uploadedVideoFile, videoExternalLink, isShowLocalVideo, this.page.id).subscribe((res) => {
            if(res)
            {
              this.getPageIntroVideos(this.page.id);
              this.subs.sink = this.translateService.get('PAGE_INTRO.MESSAGE.SUCCESS').subscribe((res) => {
                this.toastr.success(res);
              });
            }
          });
        }
        else{
          this.subs.sink = this.tokenPageService.updatePageIntroVideoUrl(videoExternalLink, isShowLocalVideo, this.page.id).subscribe((res) => {
            if(res)
            {
              this.getPageIntroVideos(this.page.id);
              this.subs.sink = this.translateService.get('PAGE_INTRO.MESSAGE.SUCCESS').subscribe((res) => {
                this.toastr.success(res);
              });
            }
          });
        }
      }
    }
    else{
      this.subs.sink = this.translateService.get('PAGE_INTRO.MESSAGE.VALIDATION').subscribe((res) => {
        this.toastr.error(res);
      });
    }
  }

  showUploadedVideo(e): void {
    if(this.pageIntroVideos.videoUrl){
      const modalRef = this.modalService.open(PageIntroVideoComponent, {"modalDialogClass":"page-into-modal"});
      modalRef.componentInstance.showVideoUrl = true;
      modalRef.componentInstance.videoUrl = this.pageIntroVideos.videoUrl;
      modalRef.componentInstance.videoExternalLink = this.pageIntroVideos.videoExternalLink;
    }
  }

  showExternalLinkVideo(e): void{
    if(this.pageIntroVideos.videoExternalLink){
      const modalRefs = this.modalService.open(PageIntroVideoComponent, {"modalDialogClass":"page-into-modal"});
      modalRefs.componentInstance.showVideoUrl = false;
      modalRefs.componentInstance.videoUrl = this.pageIntroVideos.videoUrl;
      modalRefs.componentInstance.videoExternalLink = this.pageIntroVideos.videoExternalLink;
    }
  }

}
