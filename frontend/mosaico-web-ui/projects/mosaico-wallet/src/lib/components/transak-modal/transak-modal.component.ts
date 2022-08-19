import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { SubSink } from 'subsink';
import { BlockchainNetworkType, ConfigService, DialogBase } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TransactionService } from '../../services/transaction.service';
import transakSDK from '@transak/transak-sdk'
import { TransakUserKYC } from '../../models';

@Component({
  selector: 'lib-transak-modal',
  templateUrl: './transak-modal.component.html',
  styleUrls: ['./transak-modal.component.scss']
})
export class TransakModalComponent extends DialogBase implements OnInit, OnDestroy {
  @Input() user: TransakUserKYC;
  @Input() network: BlockchainNetworkType = "Ethereum";
  @Input() walletAddress: string;
  @Input() fiatAmount: number = 0
  @Input() fiatCurrency: string = 'USD';
  @Output() transactionConfirmed = new EventEmitter<any>();
  @Output() closed = new EventEmitter<any>();
  
  transakUrl: SafeResourceUrl;
  subsink: SubSink = new SubSink();
  currentTransactionId: string;

  constructor(modalService: NgbModal, private configService: ConfigService, private sanitizer: DomSanitizer, private transactionService: TransactionService) {
    super(modalService);
  }

  ngOnDestroy(): void {
    this.subsink.unsubscribe();
  }

  ngOnInit(): void {
  }

  open(): void {
    const config = this.configService.getConfig().transak;

    //@ts-ignore
    let transak = new transakSDK({
      ...config,
      ...this.user,
      walletAddress: this.walletAddress,
      network: this.getActiveNetwork(this.network),
      networks: this.getActiveNetwork(this.network),
      defaultCryptoCurrency: 'USDT',
      countryCode: 'PL',
      cryptoCurrencyList: 'USDT,USDC,MATIC',
      fiatAmount: this.fiatAmount,
      fiatCurrency: this.fiatCurrency
    });

    //@ts-ignore
    transak.init();
    transak.on(transak.EVENTS.TRANSAK_WIDGET_CLOSE, (orderData: any) => {
      transak.close();
      this.closed.emit();
    });
    transak.on(transak.EVENTS.TRANSAK_ORDER_CREATED, (orderData: any) => {
      this.confirmTransaction(orderData);
    });
  }

  confirmTransaction(orderData: any): void {
    this.transactionConfirmed.emit(orderData);
  }

  private getActiveNetwork(network: BlockchainNetworkType): string {
    switch(network) {
      case 'Polygon':
        return 'matic';
      default:
        return 'ethereum';
    }
  }

}
