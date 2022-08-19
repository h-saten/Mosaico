import { Component, OnInit } from '@angular/core';
import { SwiperOptions } from 'swiper';

import { CreateStepItem } from '../../../models';

@Component({
  selector: 'app-section-follow-steps-create',
  templateUrl: './section-follow-steps-create.component.html',
  styleUrls: ['./section-follow-steps-create.component.scss']
})
export class SectionFollowStepsCreateComponent implements OnInit {
  createSteps: CreateStepItem[] = [
    {
      title: 'FSC.title_1',
      description: 'FSC.desc_1',
      action: 'FSC.action_1',
      icon: '/assets/media/mainpage/pencil-doc.png',
      link: '/about',
      currenStep: 1
    },
    {
      title: 'FSC.title_2',
      description: 'FSC.desc_2',
      action: 'FSC.action_2',
      icon: '/assets/media/mainpage/men-headphone.png',
      link: '/about',
      currenStep: 2
    },
    {
      title: 'FSC.title_3',
      description: 'FSC.desc_3',
      action: 'FSC.action_3',
      icon: '/assets/media/mainpage/rocket.png',
      link: '/projects',
      currenStep: 3
    },
    {
      title: 'FSC.title_4',
      description: 'FSC.desc_4',
      action: 'FSC.action_4',
      icon: '/assets/media/mainpage/people.png',
      link: '/projects',
      currenStep: 4
    },
    {
      title: 'FSC.title_5',
      description: 'FSC.desc_5',
      action: 'FSC.action_5',
      icon: '/assets/media/mainpage/transfer.png',
      link: '/dex',
      currenStep: 5
    },
    {
      title: 'FSC.title_6',
      description: 'FSC.desc_6',
      action: 'FSC.action_6',
      icon: '/assets/media/mainpage/desktop.png',
      link: '/projects',
      currenStep: 6
    }
  ];

  dotsArray = Array(66).fill(null);
  dotsSmallArray = Array(17).fill(null);

  config: SwiperOptions = {
    slidesPerView: 'auto',
    spaceBetween: 18,
    centeredSlides: true,
    slideToClickedSlide: true
  }

  constructor() { }

  ngOnInit(): void {
  }

}
