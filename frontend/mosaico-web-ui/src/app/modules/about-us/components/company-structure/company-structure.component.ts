import { Component, OnInit } from '@angular/core';

import {TranslateService} from '@ngx-translate/core';
import SwiperCore, { Pagination, SwiperOptions } from "swiper";

import { ProductItem } from './../models/index';

SwiperCore.use([Pagination]);

@Component({
  selector: 'app-company-structure',
  templateUrl: './company-structure.component.html',
  styleUrls: ['./company-structure.component.scss']
})
export class CompanyStructureComponent implements OnInit {
  config: SwiperOptions = {
    spaceBetween: 11,
    slidesPerView: 'auto',
    loop: true,
    slideToClickedSlide: true,
    pagination: {
      clickable: true
    }
  };

  product: ProductItem = {
    id: 0,
    title: 'Product',
    icon: '/assets/media/our-team/icons/noun-coding.svg',
    icon_active: '/assets/media/our-team/icons/noun-coding-active.svg',
    teamLeades: [
      {
        name: 'Dmytro Morozov',
        position: 'CTO',
        image: '/assets/media/our-team/dm.png'
      }
    ],
    teamMembers: [
      {
        name: 'Klaudia Shleha',
        position: 'Product Owner',
        image: '/assets/media/our-team/kl.png'
      },
      {
        name: 'Michał Czaja',
        position: 'Devops Engineer',
        image: '/assets/media/our-team/mi.png'
      },
      {
        name: 'Tetiana Pelykh',
        position: 'UX / UI Designer',
        image: '/assets/media/our-team/tiana.png'
      },
      {
        name: 'Yaroslaw Pankratiev',
        position: 'QA',
        image: '/assets/media/our-team/yar.png'
      },
      {
        name: 'Alina S.',
        position: 'Tester',
        image: '/assets/media/our-team/alina-stopka.png'
      },
      {
        name: 'Marcin W.',
        position: 'Mobile App Developer',
        image: '/assets/media/our-team/marcin-w.png'
      }
    ]
  };

  marketing: ProductItem = {
    id: 1,
    title: 'Marketing',
    icon: '/assets/media/our-team/icons/noun-diagram.svg',
    icon_active: '/assets/media/our-team/icons/noun-diagram-active.svg',
    teamLeades: [
      {
        name: 'Tomasz Dziedzic',
        position: 'CMO',
        image: '/assets/media/our-team/tomasz-dziedzic.png'
      }
    ],
    teamMembers: [
      {
        name: 'Rafal Świaczny',
        position: 'Brand manager Mosaico & Sapiency',
        image: '/assets/media/our-team/rafal-swiaczny.png'
      },
      {
        name: 'Ela Chwistek',
        position: 'Graphic Designer',
        image: '/assets/media/our-team/el.png'
      },
      {
        name: 'Karol Kisielewski',
        position: 'Social Media Moderator',
        image: '/assets/media/our-team/karol-kisielewski.png'
      }
    ]
  };

  issuingDepartment: ProductItem = {
    id: 2,
    title: 'ORG_STRUCTURE.ISSUING_DEPARTMENT',
    icon: '/assets/media/our-team/icons/desktop.svg',
    icon_active: '/assets/media/our-team/icons/desktop-active.svg',
    teamLeades: [
      {
        name: 'Radoslaw Ordyniec',
        position: 'CSO',
        image: '/assets/media/our-team/radek.png'
      },
    ],
    teamMembers: [
      {
        name: 'Marcin Walkowski',
        position: 'Project Manager',
        image: '/assets/media/our-team/marcin-walkowski.png'
      },
      {
        name: 'Przemysław Kołaziński',
        position: 'New Business Developer',
        image: '/assets/media/our-team/prz.png'
      },
      {
        name: 'Kamil Ciastoń',
        position: 'New Business Developer',
        image: '/assets/media/our-team/kamil.png'
      },
      {
        name: 'Mikołaj Januś',
        position: 'ICO Project Manager',
        image: '/assets/media/our-team/janus.png'
      },
      {
        name: 'Piotr Sobczyk',
        position: 'New Business Developer Contact with emitents',
        image: '/assets/media/our-team/piotr-sobczyk.png'
      },
      {
        name: 'Dorota Rogowska',
        position: 'Copywritter',
        image: '/assets/media/our-team/dorota-rogowska.png'
      }
    ]
  };

  cfo: ProductItem = {
    id: 3,
    title: 'CFO',
    icon: '/assets/media/our-team/icons/portfolio.svg',
    icon_active: '/assets/media/our-team/icons/portfolio-active.svg',
    teamMembers: [
      {
        name: 'Rafal Paciorek',
        position: 'CFO',
        image: '/assets/media/our-team/rafal.png'
      },
      {
        name: 'Aneta Liberda',
        position: 'Administration & Finance Specialist',
        image: '/assets/media/our-team/aneta.png'
      }
    ]
  };

  legal: ProductItem = {
    id: 4,
    title: 'Legal',
    icon: '/assets/media/our-team/icons/balance.svg',
    icon_active: '/assets/media/our-team/icons/balance-active.svg',
    teamMembers: [
      {
        name: 'Konrad Chudzik',
        position: 'Lawyer',
        image: '/assets/media/our-team/konrad-chudzik.png'
      },
      {
        name: 'Małgorzata Fituch',
        position: 'Lawyer',
        image: '/assets/media/our-team/malgorzata-fituch.png'
      }
    ]
  };

  hr: ProductItem = {
    id: 5,
    title: 'HR',
    icon: '/assets/media/our-team/icons/microscope.svg',
    icon_active: '/assets/media/our-team/icons/microscope-active.svg',
    teamMembers: [
      {
        name: 'Agnieszka Sutowicz',
        position: 'People & Cutlure Manager',
        image: '/assets/media/our-team/agni.png'
      },
      {
        name: 'Aleksandra Kłos',
        position: 'HR Specialist',
        image: '/assets/media/our-team/klos.png'
      }
    ]
  };

  advisors: ProductItem = {
    id: 6,
    title: 'ORG_STRUCTURE.ADVISORS',
    icon: '/assets/media/our-team/icons/tie.svg',
    icon_active: '/assets/media/our-team/icons/tie-active.svg',
    teamMembers: [
      {
        name: 'Wojciech Kaszycki',
        position: 'Founder & Chairman - Mobilum <br> Advisor',
        image: '/assets/media/our-team/Wojciech-Kaszycki-image.png'
      },
      {
        name: 'Sławomir Zawadzki',
        position: 'CEO - Kanga.exchange <br> Advisor',
        image: '/assets/media/our-team/Sławomir-Zawadzki-image.png'
      }
    ]
  };

  teamMembersArr = [this.product, this.marketing, this.issuingDepartment, this.cfo, this.legal, this.hr, this.advisors];
  teamId: number = 0;

  constructor(private translate: TranslateService) { }

  ngOnInit(): void {
  }

  onChangeTeam(id: number) {
    this.teamId = id;
  }
}
