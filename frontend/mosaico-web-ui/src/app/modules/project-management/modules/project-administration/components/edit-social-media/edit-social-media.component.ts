import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { ErrorHandlingService, TranslationService, validateForm, PatchModelSocialLinks, FormBase } from 'mosaico-base';
import { Page, ProjectSocialMedia, TokenPageService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { selectProjectPage } from 'src/app/modules/project-management/store';
import { languages, LanguageFlag } from 'src/app/modules/shared/models';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-edit-social-media',
  templateUrl: './edit-social-media.component.html',
  styleUrls: ['./edit-social-media.component.scss']
})
export class EditSocialMediaComponent extends FormBase implements OnInit, OnDestroy {

  private page: Page;

  dataSavingRequestInProgress = false;
  isEditingIsInProgress = false;

  mainForm: FormGroup;
  companyId: string;

  loading = false;

  subs: SubSink = new SubSink();

  projectSocialMedia: ProjectSocialMedia;

  langs = languages;
  language: LanguageFlag;
  currentLang: string;

  inProcessOfEditing = false;

  initialStateOfForm = null;

  isDirty$: Observable<boolean>;

  constructor(
    public activeModal: NgbActiveModal,
    private pageService: TokenPageService,
    private formBuilder: FormBuilder,
    private store: Store,
    private toastrService: ToastrService,
    private errorHandling: ErrorHandlingService,
    private translationService: TranslationService,
    private translateService: TranslateService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getLanguage();
    this.createForm();
    this.getSocialLinks();

  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;

    this.language = this.translationService.getLanguage(this.currentLang);

    this.subs.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;

        this.language = this.translationService.getLanguage(this.currentLang);
      });
  }

  // an attempt to capture a form change - if the form is changed, you can activate the Save button
  selectLanguage(lang: string): void {

    // console.log(this.mainForm);
    // console.log('touched: ', this.mainForm.touched);
    // console.log('dirty: ', this.mainForm.dirty);
    // console.log('pristine: ', this.mainForm.pristine);
    // console.log('validator: ', this.mainForm.validator);

    // this.mainForm.valueChanges.subscribe((res) => {

    //   console.log('res: ', res);
    //   if (res && res.length > 0) {
    //   }
    // });

    // this.initialStateOfForm;


    // this.translationService.setLanguage(lang);
  }

  setInProcessOfEditing(): void {
    // this.inProcessOfEditing = true;

    // console.log('this.inProcessOfEditing: ', this.inProcessOfEditing);
  }

  private createForm(): void {
    const reg = /^(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})$/;
    this.mainForm = this.formBuilder.group({
      telegram: ['', [Validators.pattern(reg)]],
      youtube: ['', [Validators.pattern(reg)]],
      linkedin: ['', [Validators.pattern(reg)]],
      facebook: ['', [Validators.pattern(reg)]],
      twitter: ['', [Validators.pattern(reg)]],
      instagram: ['', [Validators.pattern(reg)]],
      medium: ['', [Validators.pattern(reg)]],
      tiktok: ['', [Validators.pattern(reg)]],
      pageurl: ['', [Validators.pattern(reg)]]
    });

    // this.mainForm = new FormGroup({
    //   telegram: new FormControl()
    // })
  }

  get m(){
    return this.mainForm.controls;
  }

  private getSocialLinks(): void {
      // it has to be here - because the main component won't deliver the data the second time you open the modal without reloading the page
      this.subs.sink = this.store.select(selectProjectPage).subscribe((res: Page) => {
      if (res) {
        this.page = res;

        let social: ProjectSocialMedia = {
          telegram: '',
          youtube: '',
          linkedin: '',
          facebook: '',
          twitter: '',
          instagram: '',
          medium: '',
          tiktok: '',
          pageurl: ''
        }

        this.page.socialMediaLinks.forEach((link) => {
          switch (link.key) {
            case 'telegram':
              social.telegram = link.value;
              break;
            case 'youtube':
              social.youtube = link.value;
              break;
            case 'linkedin':
              social.linkedin = link.value;
              break;
            case 'facebook':
              social.facebook = link.value;
              break;
            case 'twitter':
              social.twitter = link.value;
              break;
            case 'instagram':
              social.instagram = link.value;
              break;
            case 'medium':
              social.medium = link.value;
              break;
            case 'tiktok':
              social.tiktok = link.value;
              break;
            case 'pageurl':
              social.pageurl = link.value;
              break;
          }
        });

        this.updateFormValue(social);
      }
    });
  }

  private updateFormValue(social: ProjectSocialMedia): void {
    this.mainForm.setValue({
      telegram: social.telegram ? social.telegram : null,
      youtube: social.youtube,
      linkedin: social.linkedin,
      facebook: social.facebook,
      twitter: social.twitter,
      instagram: social.instagram,
      medium: social.medium,
      tiktok: social.tiktok,
      pageurl: social.pageurl
    });

    this.initialStateOfForm = social;
  }

  save(): void {
    this.loading = true;

    // let command = this.mainForm.getRawValue();
    if (validateForm(this.mainForm)) {
      // command = trim(command);

      if (this.page && this.page.id) {
        this.dataSavingRequestInProgress = true;

        const updateExpression: PatchModelSocialLinks = {
          "path": "/SocialMediaLinks/" + this.currentLang,
          "op": "add",
          "value": []
        };

        let order = 0;

        if (this.mainForm.value.telegram) {
          this.setObjectSocialLinks(updateExpression, "telegram", this.mainForm.value.telegram, false, order);
          order++;
        }

        if (this.mainForm.value.youtube) {
          this.setObjectSocialLinks(updateExpression, "youtube", this.mainForm.value.youtube, false, order);
          order++;
        }

        if (this.mainForm.value.linkedin) {
          this.setObjectSocialLinks(updateExpression, "linkedin", this.mainForm.value.linkedin, false, order);
          order++;
        }

        if (this.mainForm.value.facebook) {
          this.setObjectSocialLinks(updateExpression, "facebook", this.mainForm.value.facebook, false, order);
          order++;
        }

        if (this.mainForm.value.twitter) {
          this.setObjectSocialLinks(updateExpression, "twitter", this.mainForm.value.twitter, false, order);
          order++;
        }

        if (this.mainForm.value.instagram) {
          this.setObjectSocialLinks(updateExpression, "instagram", this.mainForm.value.instagram, false, order);
          order++;
        }

        if (this.mainForm.value.medium) {
          this.setObjectSocialLinks(updateExpression, "medium", this.mainForm.value.medium, false, order);
          order++;
        }

        if (this.mainForm.value.tiktok) {
          this.setObjectSocialLinks(updateExpression, "tiktok", this.mainForm.value.tiktok, false, order);
          order++;
        }

        if (this.mainForm.value.pageurl) {
          this.setObjectSocialLinks(updateExpression, "pageurl", this.mainForm.value.pageurl, false, order);
          order++;
        }

        this.subs.sink = this.pageService.patchPageSocialLinks(this.page.id, [updateExpression]).subscribe(() => {
          this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.SOCIAL_MEDIA_UPDATED').subscribe((res) => {
            this.toastrService.success(res);
          });

          this.activeModal.close();
        }, (error) => {this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false;} );

      } else {
        // this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.INVALID_FORM').subscribe((res) => {
        //   this.toastrService.error(res);
        // });
      }

    } else {
      this.translateService.get('SOCIAL_LINKS.MESSAGE.FAILED').subscribe((t) => {
        this.toastrService.error(t);
        this.loading = false;
      });
    }
  }

  private setObjectSocialLinks(updateExpression: PatchModelSocialLinks, key: string, value: string, isHioden: boolean, order: number): void {
    updateExpression.value.push ({
      "key": key,
      "value": value,
      "isHidden": isHioden,
      "order": order
    });
  }


}
