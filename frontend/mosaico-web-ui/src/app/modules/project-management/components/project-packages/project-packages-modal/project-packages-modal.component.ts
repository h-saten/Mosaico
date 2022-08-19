import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormModeEnum, TranslationService } from 'mosaico-base';
import { ProjectPackage } from 'mosaico-project';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import { Store } from '@ngrx/store';
import { selectPreviewProjectPermissions } from '../../../store';
import { LanguageFlag, languages } from 'src/app/modules/shared/models';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';

@Component({
  // selector: 'app-project-packages-modal',
  templateUrl: './project-packages-modal.component.html',
  styleUrls: ['./project-packages-modal.component.scss']
})
export class ProjectPackagesModalComponent implements OnInit, OnDestroy {

  @Input() packageId?: string;
  @Input() projectId: string;
  @Input() pageId: string;

  itemList: ProjectPackage[];

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  @Input() currentFormMode: FormModeEnum;

  isEditingIsInProgress = false;
  dataSavingRequestInProgress = false;

  private subs: SubSink = new SubSink();

  langs = languages;
  language: LanguageFlag;
  currentLang: string;

  canEdit = false;;
  editEnabled = false;

  constructor(
    public activeModal: NgbActiveModal,
    private store: Store,
    private translationService: TranslationService,
    private translateService: TranslateService
  ) {
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });

    this.getLanguage();
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

  selectLanguage(lang: string): void {
    this.translationService.setLanguage(lang);
  }

  saveEventFromForm(event: boolean): void {
    this.activeModal.close(true); // modal with form - the signal will be read at the bottom in modalRef.closed.subscribe ()
  }

  cancelEventFromForm(event: boolean): void {
    this.activeModal.close(false);
  }


}

