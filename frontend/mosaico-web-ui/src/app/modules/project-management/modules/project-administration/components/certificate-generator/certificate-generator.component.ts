import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {
  CertificateConfiguration,
  CertificateConfigurationResponse,
  ProjectCertificateService,
  UpsertCertificateConfigurationCommand
} from "mosaico-project";
import {SubSink} from "subsink";
import {selectProjectPreview} from "../../../../store";
import {Store} from "@ngrx/store";
import {finalize} from "rxjs/operators";
import {FileParameter} from "./certificate-editor/models";
import {LanguageFlag, languages} from "../../../../../shared/models";
import {FileResponse, TranslationService} from "mosaico-base";

@Component({
  selector: 'app-certificate-generator',
  templateUrl: './certificate-generator.component.html',
  styles: ['']
})
export class CertificateGeneratorComponent implements OnInit, OnDestroy {

  subs: SubSink = new SubSink();

  projectId = '';
  tokenSymbol = '';
  logoUrl: string;

  certificateConfig: CertificateConfigurationResponse = null;

  configurationRequestFinalized = false;
  canShowEditor = false;

  langs = languages;
  language: LanguageFlag;

  certificatePdf: Blob = null;

  constructor(
    private notifier: ToastrService,
    private cd: ChangeDetectorRef,
    private projectCertificateService: ProjectCertificateService,
    private store: Store,
    private toastr: ToastrService,
    private translationService: TranslationService
  ) {
    this.setLanguage(this.translationService.getSelectedLanguage(), true);
  }

  ngOnInit(): void {
    this.getProject();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private canShowEditorCheck(): boolean {
    return this.tokenSymbol?.length > 0
      && this.configurationRequestFinalized === true
      && this.certificateConfig !== null;
  }

  selectLanguage(lang: string, initialSetting = false): void {
    this.setLanguage(lang, initialSetting);
  }

  setLanguage(lang: string, initialSetting: boolean): void {
    this.langs.forEach((language: LanguageFlag) => {
      if (language.lang === lang) {
        language.active = true;
        const prevLang = this.language?.lang;
        this.language = language;

        if (!initialSetting && prevLang !== this.language.lang) {
          this.getConfiguration(this.projectId, this.language.lang);
        }

      } else {
        language.active = false;
      }
    });
  }

  private getProject(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.projectId = data.project.id;
        this.logoUrl = data.project.logoUrl;
        this.tokenSymbol = data.token?.symbol;
        this.getConfiguration(this.projectId, this.language.lang);
        this.canShowEditor = this.canShowEditorCheck();
      }
    });
  }

  private getConfiguration(projectId: string, language: string): void {
    this.configurationRequestFinalized = false;
    this.subs.sink = this.projectCertificateService
      .getCertificateConfiguration(projectId, language)
      .pipe(finalize(() => {
        this.configurationRequestFinalized = true;
        this.canShowEditor = this.canShowEditorCheck();
      }))
      .subscribe(response => {
        this.certificateConfig = response.data;
      });
  }

  private upsertConfiguration(projectId: string, command: UpsertCertificateConfigurationCommand): void {
    this.subs.sink = this.projectCertificateService
      .upsertCertificateConfiguration(projectId, command)
      .pipe(finalize(() => {
        // finalized
      }))
      .subscribe(() => {
        this.toastr.success("Certificate configuration changed.");
      });
  }

  private uploadBackground(projectId: string, file: File, language: string): void {
    this.subs.sink = this.projectCertificateService
      .uploadCertificateBackground(projectId, file, language)
      .pipe(finalize(() => {
        // finalized
      }))
      .subscribe(() => {
        this.toastr.success("Certificate background image changed.");
      });
  }

  private getExampleCertificatePreview(projectId: string, language: string): void {
    this.subs.sink = this.projectCertificateService
      .getExampleCertificatePdf(projectId, language)
      .pipe(finalize(() => {
        // finalized
      }))
      .subscribe((response) => {
        this.certificatePdf = response.data.data;
      });
  }

  saveNewConfiguration(newConfig: CertificateConfiguration) {
    this.upsertConfiguration(this.projectId, {
      code: newConfig.code,
      date: newConfig.date,
      logo: newConfig.logoBlock,
      name: newConfig.investorName,
      tokens: newConfig.tokensAmount,
      sendingEnabled: this.certificateConfig.sendCertificateToInvestor
    });
  }

  toggleSendingConfiguration(sendingEnabled: boolean): void {
    const currentConfiguration = this.certificateConfig.configuration;
    this.upsertConfiguration(this.projectId, {
      code: currentConfiguration.code,
      date: currentConfiguration.date,
      logo: currentConfiguration.logoBlock,
      name: currentConfiguration.investorName,
      tokens: currentConfiguration.tokensAmount,
      sendingEnabled: sendingEnabled
    });
  }

  saveNewBackgroundImage(backgroundImageFile: FileParameter): void {
    this.uploadBackground(this.projectId, backgroundImageFile.data, this.language.lang);
  }

  generateExampleCertificate(): void {
    this.getExampleCertificatePreview(this.projectId, this.language.lang);
  }
}
