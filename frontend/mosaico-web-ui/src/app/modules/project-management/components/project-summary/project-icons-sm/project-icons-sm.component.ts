import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { Page, ProjectSocialMedia, SocialMediaLinks, TokenPageService } from 'mosaico-project';
import { LanguageEnum } from 'src/app/modules/shared/models';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import { EditSocialMediaComponent } from '../../../modals/edit-social-media/edit-social-media.component';
import { selectPreviewProjectPermissions, selectProjectPage, setProjectPage } from '../../../store';

@Component({
  selector: 'app-project-icons-sm',
  templateUrl: './project-icons-sm.component.html',
  styleUrls: ['./project-icons-sm.component.scss']
})
export class ProjectIconsSmComponent implements OnInit {

  private page: Page;

  private subs: SubSink = new SubSink();
  canEdit = false;

  currentLang = '';
  languageEnum: typeof LanguageEnum = LanguageEnum;
  displayFullTextButton = true;

  projectSocialMedia: ProjectSocialMedia = {
    telegram: '',
    youtube: '',
    linkedin: '',
    facebook: '',
    twitter: '',
    instagram: '',
    medium: '',
    tiktok: '',
    pageurl: ''
  };

  links: SocialMediaLinks[] = [];

  constructor(
    private translateService: TranslateService,
    private store: Store,
    private modalService: NgbModal,
    private tokenPageService: TokenPageService
  ) { }

  ngOnInit(): void {

    this.getProjectPermissions();
    this.getLanguage();

    // this.getUrlToSocialMedia();
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;
    this.getUrlToSocialMedia();

    this.subs.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;
        this.getUrlToSocialMedia();
      });
  }

  private getUrlToSocialMedia(): void {
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res) {
        this.page = res;

        const socialMediaLinks: SocialMediaLinks[] = this.page.socialMediaLinks;
        if (socialMediaLinks) {
          socialMediaLinks.forEach((link) => {
            switch (link.key) {
              case 'telegram':
                this.projectSocialMedia.telegram = link.value;
                break;
              case 'youtube':
                this.projectSocialMedia.youtube = link.value;
                break;
              case 'linkedin':
                this.projectSocialMedia.linkedin = link.value;
                break;
              // case 'FACEBOOK':
              case 'facebook':
                this.projectSocialMedia.facebook = link.value;
                break;
              case 'twitter':
                this.projectSocialMedia.twitter = link.value;
                break;
              case 'instagram':
                this.projectSocialMedia.instagram = link.value;
                break;
              case 'medium':
                this.projectSocialMedia.medium = link.value;
                break;
              case 'tiktok':
                this.projectSocialMedia.tiktok = link.value;
                break;
              case 'pageurl':
                this.projectSocialMedia.pageurl = link.value;
                break;
            }
            if(link.value && link.value.length > 0) {
              this.displayFullTextButton = false;
            }
          });
        }
      }
    });


  }

  private getProjectPermissions(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS];
    });
  }

  openEditingModal(): void {
    const modalRef = this.modalService.open(EditSocialMediaComponent, DEFAULT_MODAL_OPTIONS);
    // modalRef.componentInstance.imgUrl = this.page.coverUrl;
    // modalRef.componentInstance.pageId = this.page?.id;
    modalRef.componentInstance.page = this.page;

    this.subs.sink = modalRef.closed.subscribe(() => {
      setTimeout(() => {
        this.subs.sink = this.tokenPageService.getPage(this.page.id).subscribe((response) => {
          if(response && response.data) {
            this.store.dispatch(setProjectPage({page: response.data}));
          }
        });
      }, 1000);
    });
  }

}
