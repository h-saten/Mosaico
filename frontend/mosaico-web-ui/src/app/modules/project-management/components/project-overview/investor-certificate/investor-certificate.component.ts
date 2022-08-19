import {Component, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {TranslateService} from '@ngx-translate/core';
import {ToastrService} from 'ngx-toastr';
import {selectProjectPreview} from '../../../store';
import {saveAs} from 'file-saver';
import {finalize} from "rxjs/operators";
import {ProjectCertificateService} from "mosaico-project";
import {SubSink} from "subsink";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {InvestorCertificateLoaderComponent} from "./investor-certificate-loader/investor-certificate-loader.component";
import {DEFAULT_MODAL_OPTIONS} from "mosaico-base";
import {NgbModalRef} from "@ng-bootstrap/ng-bootstrap/modal/modal-ref";

@Component({
  selector: 'app-investor-certificate',
  templateUrl: './investor-certificate.component.html',
  styleUrls: ['./investor-certificate.component.scss']
})
export class InvestorCertificateComponent implements OnInit, OnDestroy {

  private subs: SubSink = new SubSink();

  projectId: string;
  isLoaded = false;
  downloadingInProgress = false;
  certificatePdf: Blob;

  loaderModal: NgbModalRef;

  constructor(
    private store: Store,
    private translateService: TranslateService,
    private toastr: ToastrService,
    private projectCertificateService: ProjectCertificateService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((res) => {
      if (res) {
        this.projectId = res.project.id;
        this.isLoaded = true;
      }
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  downloadCertificate(): void {

    if (this.downloadingInProgress) return;

    this.downloadingInProgress = true;
    this.openLoaderModal();
    this.subs.sink = this.projectCertificateService
      .getUserCertificatePdf(this.projectId, this.translateService.currentLang)
      .pipe(finalize(() => {
        this.downloadingInProgress = false;
        this.loaderModal?.close();
      }))
      .subscribe((response) => {
        this.certificatePdf = response.data.data;
        saveAs(this.certificatePdf, 'certificate.pdf');
      });
  }

  private openLoaderModal(): void {
    const options = DEFAULT_MODAL_OPTIONS;
    options.size = 'sm';
    options.keyboard = false;
    this.loaderModal = this.modalService.open(InvestorCertificateLoaderComponent, options);
  }

}
