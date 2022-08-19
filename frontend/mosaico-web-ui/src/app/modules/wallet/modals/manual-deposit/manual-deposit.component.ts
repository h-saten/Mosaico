import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormDialogBase } from 'mosaico-base';
import { BehaviorSubject } from 'rxjs';
import { SubSink } from 'subsink';
import EthereumQRPlugin from 'ethereum-qr-code';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { Store } from '@ngrx/store';
import { selectCurrentActiveBlockchains } from '../../../../store/selectors';

@Component({
  selector: 'app-manual-deposit',
  templateUrl: './manual-deposit.component.html',
  styleUrls: ['./manual-deposit.component.scss']
})
export class ManualDepositComponent extends FormDialogBase implements OnInit, OnDestroy {

  isLoading$ = new BehaviorSubject<boolean>(false);
  subs = new SubSink();
  @Input() walletAddress: string;
  @Input() network: string;
  qrImage: string;

  constructor(modal: NgbModal, private translateService: TranslateService, private toastr: ToastrService, private store: Store) {
    super(modal);
    this.extraOptions = {
      modalDialogClass: "mosaico-payment-modal"
    };
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
  }

  open(payload?: any): void {
    this.reset();
    this.isLoading$.next(true);
    super.open(payload);
    this.generateQR();
  }

  private reset(): void {
    this.qrImage = null;
  }

  private generateQR(): void {
    if (this.walletAddress && this.walletAddress.length > 0) {
      this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((res) => {
        const chainId = +res?.find((c) => c.name === this.network)?.chainId;
        if(chainId > 0) {
          const qr = new EthereumQRPlugin();
          const qrCode = qr.toDataUrl({
            to: this.walletAddress,
            chainId
          });
          qrCode.then((qrCodeDataUri) => {
            this.qrImage = qrCodeDataUri?.dataURL;
          });
        }
      });
      
    }
  }

  public save(): void {
    this.modalRef.close();
  }

  public onCopied(): void {
    this.subs.sink = this.translateService.get('MANUAL_DEPOSIT.ADDRESS_COPIED').subscribe((t) => {
      this.toastr.success(t);
    });
  }

}
