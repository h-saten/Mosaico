import { Component, OnInit } from '@angular/core';
import SwiperCore, { Pagination, SwiperOptions } from "swiper";

import { WalletItem } from './../../models/index';

SwiperCore.use([Pagination]);

@Component({
  selector: 'app-section-wallet',
  templateUrl: './section-wallet.component.html',
  styleUrls: ['./section-wallet.component.scss']
})
export class SectionWalletComponent implements OnInit {
  currentHeader = 0;
  mosLink: string = '';
  assetsLink: string = '';
  tokenLink: string = '';

  config: SwiperOptions = {
    slidesPerView: 'auto',
    centeredSlides: true,
    spaceBetween: 40,
    slideToClickedSlide: true,
    pagination: {
      clickable: true
    },
    breakpoints: {
      991: {
        spaceBetween: 128
      },
      576: {
        spaceBetween: 64
      }
    }
  };

  walletContent: WalletItem[] =  [
    {
      title: 'WAL.title_1',
      img: '/assets/media/mainpage/landing-page-desc.png',
      description: 'WAL.desc_1',
      action: 'WAL.action_read_more',
      link: 'WAL.wallet_link'
    },
    {
      title: 'WAL.title_2',
      img: '/assets/media/mainpage/landing-page-money-plant.png',
      description: 'WAL.desc_2',
      action: 'WAL.action_read_more',
      link: 'WAL.token_link'
    },
    {
      title: 'WAL.title_3',
      img: '/assets/media/mainpage/landing-page-money-triangle.png',
      description: 'WAL.desc_3',
      action: 'WAL.action_read_more',
      link: 'WAL.assets_link',
      action_2: 'WAL.action_check_out',
      link_2: '/portfolio'
    },
    {
      title: 'WAL.title_4',
      img: '/assets/media/mainpage/landing-page-money-hour.png',
      description: 'WAL.desc_4',
      action: 'WAL.action_read_more',
      link: 'WAL.mos_link',
      action_2: 'WAL.action_stake_mos',
      link_2: '/wallet'
    }
  ];

  constructor() { }
  
  ngOnInit(): void {
  }

  changeTab(index: number) {
    this.currentHeader = index;
  }

}
