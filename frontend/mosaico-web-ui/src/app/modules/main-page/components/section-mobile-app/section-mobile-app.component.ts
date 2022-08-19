import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-section-mobile-app',
  templateUrl: './section-mobile-app.component.html',
  styleUrls: ['./section-mobile-app.component.scss']
})
export class SectionMobileAppComponent implements OnInit, OnDestroy {
  subs = new SubSink();

  navigateLink: string = '';

  constructor(private translateService: TranslateService) { }

  ngOnInit(): void {
    this.onLangChange();
    
    this.subs.sink = this.translateService.onLangChange.subscribe(_ => {
      this.onLangChange();
    })
  }

  onLangChange() {
    this.navigateLink = this.translateService.instant('MAP.link')
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
