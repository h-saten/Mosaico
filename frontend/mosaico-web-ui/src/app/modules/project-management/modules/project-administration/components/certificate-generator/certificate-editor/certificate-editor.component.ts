import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import { TranslateService } from '@ngx-translate/core';
import {NgbNavChangeEvent} from '@ng-bootstrap/ng-bootstrap';
import {
  BaseBlock,
  CertificateConfiguration,
  CertificateConfigurationResponse,
  LogoBlock,
  ProjectCertificateService,
  TextBlock,
  TokensAmount,
  UpsertCertificateConfigurationCommand
} from "mosaico-project";
import {FileParameter} from "./models";
import {SubSink} from "subsink";
import {saveAs} from 'file-saver';
import {Observable} from "rxjs";
import {finalize} from "rxjs/operators";
import {LanguageFlag} from "../../../../../../shared/models";
import {CertificateConfigurationScalerService} from "./certificate-configuration-scaler.service";

@Component({
  selector: 'app-certificate-editor',
  templateUrl: './certificate-editor.component.html',
  styleUrls: ['./certificate-editor.component.scss'],
  providers: [CertificateConfigurationScalerService]
})
export class CertificateEditorComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {

  @Input() config: CertificateConfigurationResponse;
  @Input() tokenSymbol: string;
  @Input() logoUrl: string;
  @Input() certificatePreview: Blob | null;
  @Input() projectId: string;
  @Input() language: LanguageFlag;

  @Output() configurationChanged = new EventEmitter<CertificateConfiguration>();
  @Output() sendingCertificationToggled = new EventEmitter<boolean>();
  @Output() backgroundImageUpdated = new EventEmitter<FileParameter>();
  @Output() exampleCertificateRequested = new EventEmitter<void>();

  @ViewChild('certificatePlayground') certificateContainer : ElementRef;

  private subs: SubSink = new SubSink();

  readonly maxWidth = 1528;
  readonly maxHeight = 1080;
  readonly maxFileSize = 3 * 1024 * 1024;
  certificateContainerHeight = 1080;
  scale = 1;

  configurationLoaded = true;

  canUploadNewBackground = false;

  initConfigurationLoaded = false;

  showInPreviewMode = false;

  certificateBackgroundUrl: string;
  certificateBackground: File;

  investorName: TextBlock;
  certificateDate: TextBlock;
  certificateNumber: TextBlock;
  tokensAmount: TokensAmount;
  logoBlock: LogoBlock;

  editedElementTabIndex = 0;

  currentDate: Date = new Date();

  getCertificatePdfRequestInProgress = false;

  sendCertificateToInvestor = false;
  editCertificateSendingInProgress = false;

  active = 1;

  certificatePdf: Blob = null;

  requestInProgress = false;
  certificatePreviewInProgress = false;
  saveConfigurationInProgress = false;

  constructor(
    private notifier: ToastrService,
    private cd: ChangeDetectorRef,
    private projectCertificateService: ProjectCertificateService,
    private toastr: ToastrService,
    private configurationScaler: CertificateConfigurationScalerService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.getConfiguration();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.config) {
      this.getConfiguration();
    }
  }

  ngAfterViewInit(): void {
    this.detectSizesAndScaleData();
    this.cd.detectChanges();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
      if (!blob) {
        observer.next("");
        observer.complete();
      } else {
        let reader = new FileReader();
        reader.onload = event => {
          observer.next((<any>event.target).result);
          observer.complete();
        };
        reader.readAsText(blob);
      }
    });
  }

  private detectSizesAndScaleData(): void {
    const width = this.certificateContainer.nativeElement.offsetWidth;
    this.certificateContainerHeight = Math.round((width * this.maxHeight) / this.maxWidth);
    this.scale = width / this.maxWidth;
    this.rescaleConfiguration(this.scale);
  }

  private getConfiguration(): void {

    this.configurationLoaded = true;

    if (this.config) {
      this.certificateBackgroundUrl = this.config.backgroundUrl;
      this.rescaleConfiguration(this.scale);
      this.initConfigurationLoaded = true;
      this.sendCertificateToInvestor = this.config.sendCertificateToInvestor;
      this.configurationLoaded = false;
    }
  }

  dataTokensAmountCertificateBlockEvent(data: TokensAmount): void {
    if (data) {
      this.tokensAmount = data;
    }
  }

  dataLogoBlockEvent(data: LogoBlock): void {
    if (data) {
      this.logoBlock = data;
    }
  }

  private rescaleConfiguration(scale: number): void {

    const scaledResponse = this.configurationScaler.scaleTo(this.config.configuration, scale);
    this.investorName = scaledResponse.investorName;
    this.certificateDate = scaledResponse.date;
    this.tokensAmount = scaledResponse.tokensAmount;
    this.certificateNumber = scaledResponse.code;
    this.logoBlock = scaledResponse.logoBlock;
  }

  private rescaleSizesToOriginal(): CertificateConfiguration {
    const currentScale = this.scale;
    const originalScale = 1;

    return {
      investorName: {
        marginTop: Math.round(this.investorName.marginTop * originalScale / currentScale),
        marginLeft: Math.round(this.investorName.marginLeft * originalScale / currentScale),
        width: Math.round(this.investorName.width * originalScale / currentScale),
        height: Math.round(this.investorName.height * originalScale / currentScale),
        fontSizePx: Math.round(this.investorName.fontSizePx * originalScale / currentScale),
        textAlign: this.investorName.textAlign,
        textBold: this.investorName.textBold,
        fontColor: this.investorName.fontColor,
        enabled: this.investorName.enabled,
      },
      date: {
        marginTop: Math.round(this.certificateDate.marginTop * originalScale / currentScale),
        marginLeft: Math.round(this.certificateDate.marginLeft * originalScale / currentScale),
        width: Math.round(this.certificateDate.width * originalScale / currentScale),
        height: Math.round(this.certificateDate.height * originalScale / currentScale),
        fontSizePx: Math.round(this.certificateDate.fontSizePx * originalScale / currentScale),
        textAlign: this.certificateDate.textAlign,
        textBold: this.certificateDate.textBold,
        fontColor: this.certificateDate.fontColor,
        enabled: this.certificateDate.enabled,
      },
      tokensAmount: {
        marginTop: Math.round(this.tokensAmount.marginTop * originalScale / currentScale),
        marginLeft: Math.round(this.tokensAmount.marginLeft * originalScale / currentScale),
        width: Math.round(this.tokensAmount.width * originalScale / currentScale),
        height: Math.round(this.tokensAmount.height * originalScale / currentScale),
        fontSizePx: Math.round(this.tokensAmount.fontSizePx * originalScale / currentScale),
        textAlign: this.tokensAmount.textAlign,
        textBold: this.tokensAmount.textBold,
        fontColor: this.tokensAmount.fontColor,
        enabled: this.tokensAmount.enabled,
        attachTicker: this.tokensAmount.attachTicker,
      },
      code: {
        marginTop: Math.round(this.certificateNumber.marginTop * originalScale / currentScale),
        marginLeft: Math.round(this.certificateNumber.marginLeft * originalScale / currentScale),
        width: Math.round(this.certificateNumber.width * originalScale / currentScale),
        height: Math.round(this.certificateNumber.height * originalScale / currentScale),
        fontSizePx: Math.round(this.certificateNumber.fontSizePx * originalScale / currentScale),
        textAlign: this.certificateNumber.textAlign,
        textBold: this.certificateNumber.textBold,
        fontColor: this.certificateNumber.fontColor,
        enabled: this.certificateNumber.enabled,
      },
      logoBlock: {
        marginTop: Math.round(this.logoBlock.marginTop * originalScale / currentScale),
        marginLeft: Math.round(this.logoBlock.marginLeft * originalScale / currentScale),
        width: Math.round(this.logoBlock.width * originalScale / currentScale),
        height: Math.round(this.logoBlock.height * originalScale / currentScale),
        enabled: this.logoBlock.enabled,
        isRounded: this.logoBlock.isRounded,
      }
    } as CertificateConfiguration;
  }

  selectFile(event: any): void { // Angular 11, for stricter type
    if(!event.target.files[0] || event.target.files[0].length == 0) {
      this.notifier.error('You must select an image', 'error');
      return;
    }

    const file = event.target.files[0];

    if (file.size > this.maxFileSize) {
      this.notifier.error('Invalid image size', 'Size limit exceeded');
    }

    const mimeType = file.type;

    if (mimeType.match(/image\/*/) == null) {
      this.notifier.error('Only images are supported', 'error');
      return;
    }

    this.certificateBackground = file;

    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      const img = new Image();
      img.src = reader.result as string;
      img.onload = () => {

        if (img.width !== 1528 || img.height !== 1080) {
          let errTitle = this.translate.instant('issuer.certificate.invalid_img_resolution');
          let errDesc = this.translate.instant('issuer.certificate.invalid_img');
          this.notifier.error(errTitle, errDesc);
          return;
        }

        this.certificateBackgroundUrl = img.src;
        this.canUploadNewBackground = true;
      };
    };
  }

  saveBackground(): void {

    if (!this.certificateBackground) {
      return;
    }

    const file: FileParameter = {
      data: this.certificateBackground,
      fileName: this.certificateBackground.name
    };

    //this.backgroundImageUpdated.emit(file);
    this.saveNewBackgroundImage(file);
    this.canUploadNewBackground = false;
  }

  saveConfiguration(): void {
    const newConfiguration = this.rescaleSizesToOriginal();
    // this.configurationChanged.emit(newConfiguration);
    this.saveNewConfiguration(newConfiguration);
  }

  generatePdfPreview(): void {
    // this.exampleCertificateRequested.emit();
    this.generateExampleCertificate();
  }

  moveElement(e, element: BaseBlock): void {

    const elementHeight = this.investorName.height;
    const elementWidth = this.investorName.width;

    const marginTop = element.marginTop;
    const distance_y = e.distance.y;

    let resultMarginTop = marginTop + distance_y;
    if (resultMarginTop < 0) {
      resultMarginTop = 0;
    } else if (resultMarginTop > this.maxHeight) {
      resultMarginTop = this.maxHeight - elementHeight;
    }
    element.marginTop = resultMarginTop;

    const marginLeft = element.marginLeft;
    const distance_x = e.distance.x;

    let resultMarginLeft = marginLeft + distance_x;
    if (resultMarginLeft < 0) {
      resultMarginLeft = 0;
    } else if (resultMarginLeft > (this.maxWidth - elementWidth)) {
      resultMarginLeft = this.maxWidth - elementWidth;
    }
    element.marginLeft = resultMarginLeft;
  }

  changeTab(index: number): void {
    // return this.editedElementTabIndex = index;
    this.active = index;
  }

  onNavChange(changeEvent: NgbNavChangeEvent) {
    // if (changeEvent.nextId === 3) {
    //   changeEvent.preventDefault();
    // }
  }

  centerElement(element: BaseBlock): void {
    element.marginLeft = Math.round((this.maxWidth / 2 * this.scale) - (element.width / 2));
  }

  changeDisplayMode(): void {
    this.showInPreviewMode = !this.showInPreviewMode;
  }

  languageChanged(selectedLanguage: any) {
    // change language
  }

  editCertificateSendingConfiguration(): void {
    //this.sendingCertificationToggled.emit(!this.config.sendCertificateToInvestor);
    this.toggleSendingConfiguration(!this.config.sendCertificateToInvestor);
  }

  private upsertConfiguration(projectId: string, command: UpsertCertificateConfigurationCommand): void {
    this.requestInProgress = true;
    this.saveConfigurationInProgress = true;
    this.subs.sink = this.projectCertificateService
      .upsertCertificateConfiguration(projectId, command)
      .pipe(finalize(() => {
        // finalized
        this.requestInProgress = false;
        this.saveConfigurationInProgress = false;
      }))
      .subscribe(() => {
        this.toastr.success(this.translate.instant('issuer.certificate_generator.alert'));
      });
  }

  saveNewConfiguration(newConfig: CertificateConfiguration) {
    this.upsertConfiguration(this.projectId, {
      code: newConfig.code,
      date: newConfig.date,
      logo: newConfig.logoBlock,
      name: newConfig.investorName,
      tokens: newConfig.tokensAmount,
      sendingEnabled: this.config.sendCertificateToInvestor
    });
  }

  toggleSendingConfiguration(sendingEnabled: boolean): void {
    const currentConfiguration = this.config.configuration;
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
    this.subs.sink = this.projectCertificateService
      .uploadCertificateBackground(this.projectId, backgroundImageFile.data, this.language.lang)
      .pipe(finalize(() => {
        // finalized
      }))
      .subscribe(() => {
        this.toastr.success("Certificate background image changed.");
      });
  }

  generateExampleCertificate(): void {
    this.requestInProgress = true;
    this.certificatePreviewInProgress = true;
    this.subs.sink = this.projectCertificateService
      .getExampleCertificatePdf(this.projectId, this.language.lang)
      .pipe(finalize(() => {
        this.requestInProgress = false;
        this.certificatePreviewInProgress = false;
      }))
      .subscribe((response) => {
        this.certificatePdf = response.data.data;
        saveAs(response.data.data, 'certificate.pdf');
      });
  }
}
