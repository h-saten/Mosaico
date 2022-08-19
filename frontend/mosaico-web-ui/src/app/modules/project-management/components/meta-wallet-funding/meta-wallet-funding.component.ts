import {Router} from '@angular/router';
import {Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges} from '@angular/core';
import {SubSink} from 'subsink';
import {TransactionService} from 'mosaico-wallet';
import {Store} from '@ngrx/store';
import { BlockchainNetworkType } from 'mosaico-base';
import { selectBlockchain } from '../../../../store/selectors';
import { selectMetamaskAddress } from 'src/app/modules/wallet';

@Component({
  selector: 'app-meta-wallet-funding',
  templateUrl: './meta-wallet-funding.component.html',
  styleUrls: ['./meta-wallet-funding.component.scss']
})
export class MetaWalletFundingComponent implements OnInit, OnDestroy, OnChanges {

  @Input() projectId: string;
  @Input() paymentAmount = 0;
  @Input() tokenAmount: number;
  @Input() isTokensAmountValid = false;

  network: BlockchainNetworkType;
  sub: SubSink = new SubSink();
  wallet: string;
  currentCurrency = 'ETH';
  currentTransactionId: string;
  purchaseAllowed = true;

  constructor(private transactionService: TransactionService, private store: Store, private router: Router) {

  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectMetamaskAddress).subscribe((res) => {
      this.wallet = res;
    });
    this.sub.sink = this.store.select(selectBlockchain).subscribe((b) => {
      this.network = b;
      if(!this.network || this.network.length === 0) {
        this.network = 'Ethereum';
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.paymentAmount) {
      this.updatePaymentAllowance(changes.paymentAmount.currentValue, this.tokenAmount, this.isTokensAmountValid);
    }
    if (changes.tokenAmount) {
      this.updatePaymentAllowance(this.paymentAmount, changes.tokenAmount.currentValue, this.isTokensAmountValid);
    }
    if (changes.isTokensAmountValid) {
      this.updatePaymentAllowance(this.paymentAmount, this.tokenAmount, changes.isTokensAmountValid.currentValue);
    }
  }

  private updatePaymentAllowance(paymentAmount: number, tokenAmount: number, isTokensAmountValid: boolean): void {
    this.purchaseAllowed = paymentAmount > 0 && tokenAmount > 0 && isTokensAmountValid;
  }

  sendETH(): void {
    // this.sub.sink = this.transactionService.initPurchaseTransaction(this.wallet, this.network, { tokenAmount: this.tokenAmount, projectId: this.projectId, paymentProcessor: 'metamask', paymentCurrency: 'USDT' }).subscribe((res) => {
    //   this.currentTransactionId = res.data;
      // Moralis.Web3.transfer({ type: "native", amount: Moralis.Units.ETH(this.paymentAmount), receiver: "0xFd079A89F6894Bc13c23b722D867a11F2dE7e25b", tokenId: '1'}).then((res) => {
      //   this.sub.sink = this.transactionService.confirmPurchaseTransaction(this.currentTransactionId, {payedAmount: this.paymentAmount, currency: this.currentCurrency}).subscribe((res) => {
      //     this.router.navigateByUrl('/wallet');
      //   });
      // });
    //});
  }
}
