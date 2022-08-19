import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';
import { SwiperOptions } from 'swiper';

import { InvestItem } from '../../../models/index';

@Component({
  selector: 'app-section-follow-steps-invest',
  templateUrl: './section-follow-steps-invest.component.html',
  styleUrls: ['./section-follow-steps-invest.component.scss']
})
export class SectionFollowStepsInvestComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  
  helpBusinessLink: string = '';
  investDetails: InvestItem[];

  config: SwiperOptions = {
    slidesPerView: 'auto',
    spaceBetween: 18,
    centeredSlides: true,
    slideToClickedSlide: true
  }

  constructor(
    private translateService: TranslateService,
    private router: Router
    ) { }

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
    this.helpBusinessLink = this.translateService.instant('FSI.help_business_link');

    this.investDetails = [
      {
        title: 'FSI.title_1',
        description: 'FSI.desc_1',
        action: 'FSI.action_1',
        icon: '/assets/media/mainpage/check-out-proj.png',
        isRouting: true,
        link: '/projects'
      },
      {
        title: 'FSI.title_2',
        description: 'FSI.desc_2',
        action: 'FSI.action_2',
        icon: '/assets/media/mainpage/cup.png',
        isRouting: false,
        link: 'https://advisor.mosaico.ai/kb/pl/platforma-181831'
      },
      {
        title: 'FSI.title_3',
        description: 'FSI.desc_3',
        action: 'FSI.action_3',
        icon: '/assets/media/mainpage/plant-grow.png',
        isRouting: false,
        link: this.helpBusinessLink
      },
      {
        title: 'FSI.title_4',
        description: 'FSI.desc_4',
        action: 'FSI.action_4_1',
        action_2: 'FSI.action_4_2',
        icon: '/assets/media/mainpage/money-desk.png',
        isRouting: true,
        link: '/wallet',
        link_2: '/dex'
      }
    ];
  }

  onNavigate (item: InvestItem) {
    if(item.isRouting) {
      this.router.navigate([item.link])
    } else {
      window.open(item.link, '_blank')
    }
  }
}
