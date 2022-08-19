import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-check-out-whitepaper',
  templateUrl: './check-out-whitepaper.component.html',
  styleUrls: ['./check-out-whitepaper.component.scss']
})
export class CheckOutWhitepaperComponent implements OnInit {
  subs = new SubSink();

  navigateToWhitePaper: string = '';

  constructor(private translateService: TranslateService) { }

  ngOnInit(): void {
    this.onLangChange();

    this.subs.sink = this.translateService.onLangChange.subscribe(_ => {
      this.onLangChange();
    })
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
  
  onLangChange() {
    this.navigateToWhitePaper = this.translateService.instant('CHECK_OUT_WHITEPAPER.LINK')
  }

}
