import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { RampInstantEventTypes, RampInstantSDK } from '@ramp-network/ramp-instant-sdk';
import { BlockchainNetworkType, ConfigService, DialogBase } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SubSink } from 'subsink';
import { RampUserKYC } from '../../models/ramp-user-kyc';

@Component({
  selector: 'lib-ramp-modal',
  templateUrl: './ramp-modal.component.html',
  styleUrls: ['./ramp-modal.component.scss']
})
export class RampModalComponent extends DialogBase implements OnInit, OnDestroy, AfterViewInit {
  @Input() network: BlockchainNetworkType = "Ethereum";
  @Input() walletAddress: string;
  @Input() user: RampUserKYC;
  @Input() fiatAmount: number;
  @Input() fiatCurrency: string = 'USD';
  @Output() transactionConfirmed = new EventEmitter<any>();
  @Output() closed = new EventEmitter<any>();

  ramp: RampInstantSDK;
  subsink: SubSink = new SubSink();

  constructor(modalService: NgbModal, private configService: ConfigService) {
    super(modalService);
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subsink.unsubscribe();
  }

  ngAfterViewInit(): void {

  }

  open(): void {
    const apiKey = this.configService.getConfig()?.rampApiKey;
    const logo = window.location.origin + '/assets/media/logos/mosaico_logo.png';
    const config = {
      fiatCurrency: this.fiatCurrency,
      swapAsset: this.getSwapAsset(),
      hostLogoUrl: logo,
      hostAppName: 'Mosaico',
      userEmailAddress: this.user?.email,
      selectedCountryCode: 'PL',
      hostApiKey: apiKey,
      userAddress: this.walletAddress,
      fiatValue: this.fiatAmount?.toFixed(2)
    };
    //@ts-ignore
    this.ramp = new RampInstantSDK(config);
    this.ramp.on(RampInstantEventTypes.PURCHASE_CREATED, (event) => {
        this.confirmTransaction(event);
    });
    this.ramp.on(RampInstantEventTypes.WIDGET_CLOSE, (event) => {
      this.closed.emit(event);
    });
    this.ramp.show();
  }

  confirmTransaction(orderData: any): void {
    this.transactionConfirmed.emit(orderData);
  }

  getSwapAsset(): string {
    switch(this.network) {
      case 'Polygon':
        return 'MATIC_USDC,MATIC';
      default:
        return 'USDC,USDT';
    }
  }

}
