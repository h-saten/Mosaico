import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BlockchainService } from 'mosaico-base';
import { selectProjectPreviewToken } from '../../../store';
import { selectCurrentActiveBlockchains } from '../../../../../store/selectors';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-blockchain-links',
  templateUrl: './blockchain-links.component.html',
  styleUrls: ['./blockchain-links.component.scss']
})
export class BlockchainLinksComponent implements OnInit, OnDestroy {
  link: string;
  constructor(private blockchainService: BlockchainService, private store: Store) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  isLoaded = false;
  subs = new SubSink();

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((res) => {
      if(res && res.address){
        this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((chains) => {
          const chain = chains.find((c) => c.name === res.network);
          if(chain){
            this.link = this.blockchainService.getTokenLink(res.address, chain.etherscanUrl);
          }
          this.isLoaded = true;
        });
      }
    });
  }

  navigateToEtherscan(): void {
    if (this.isLoaded && this.link.length > 0) {      
      window.open(this.link, "_blank", 'noopener');
    }
  }

}
