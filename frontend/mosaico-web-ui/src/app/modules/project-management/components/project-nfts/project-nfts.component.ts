import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { concatMap, map, filter, takeUntil } from 'rxjs/operators';
import { BehaviorSubject, combineLatest, Observable, Subject } from 'rxjs';
import { TokenPageService } from 'projects/mosaico-project/src/lib/services/token-page.service';
import { Nft } from 'projects/mosaico-project/src/lib/models/nft';
import { Project } from 'mosaico-project';
import { BlockchainService } from 'mosaico-base';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { selectPreviewProject, selectProjectPreviewToken } from '../../store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-project-nfts',
  templateUrl: './project-nfts.component.html',
  styleUrls: ['./project-nfts.component.scss']
})
export class ProjectNftsComponent implements OnInit, OnDestroy {
  public nfts$: Observable<Nft[]>;

  subs = new SubSink();

  constructor(
    private tokenPageService: TokenPageService,
    private blockChainService: BlockchainService,
    private store: Store
  ) {}

  ngOnInit(): void {
    this.getNfts();
  }

  private getNfts(): void {
    this.nfts$ = this.store.select(selectPreviewProject).pipe(
      concatMap((project: Project) => {
        return this.tokenPageService.getNft(project.id).pipe(
          map(response => response.data.entities)
        )
      })
    );
  }

  public openScanner(nft: Nft): void {
    this.subs.sink = combineLatest([
      this.store.select(selectCurrentActiveBlockchains)
    ]).pipe(
      filter(([chains]) => !!nft && !!chains),
      map(([chains]) => {
        const chain = chains.find(chain => chain.name === nft.network);
        if (chain) {
          return this.blockChainService.getTokenLink(nft.address, chain.etherscanUrl)
        }
      })
    ).subscribe(link => {
      if(link && link.length > 0) {
        window.open(link, "_blank", 'noopener');
      }
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

}
