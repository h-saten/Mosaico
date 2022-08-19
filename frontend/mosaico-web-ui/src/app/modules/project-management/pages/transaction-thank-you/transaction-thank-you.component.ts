import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from "@angular/router";
import { SubSink } from "subsink";
import { BlockchainService } from 'mosaico-base';
import { Store } from '@ngrx/store';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';

@Component({
  selector: 'app-transaction-thank-you',
  templateUrl: './transaction-thank-you.component.html',
  styleUrls: []
})
export class TransactionThankYouComponent implements OnInit, OnDestroy {
  network: string;
  tx: string;
  public subs: SubSink = new SubSink();

  constructor(
    private translateService: TranslateService,
    private blockchainService: BlockchainService,
    private store: Store,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.network = this.route.snapshot.queryParamMap.get('network');
    this.tx = this.route.snapshot.queryParamMap.get('tx');
  }

  openEtherscan(): void {
    if (this.network?.length > 0 && this.tx?.length > 0) {
      this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((chains) => {
        const chain = chains.find((c) => c.name === this.network);
        if (chain) {
          const link = this.blockchainService.getTransactionLink(this.tx, chain.etherscanUrl);
          if (link && link.length > 0) {
            window.open(link, "_blank", 'noopener');
          }
        }
      });
    }
  }

  ngOnDestroy(): void {

  }

}
