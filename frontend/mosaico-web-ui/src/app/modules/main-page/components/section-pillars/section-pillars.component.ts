import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-section-pillars',
  templateUrl: './section-pillars.component.html',
  styleUrls: ['./section-pillars.component.scss']
})
export class SectionPillarsComponent implements OnInit {
  pillars = [
    {
      img: '/assets/media/mainpage/pillar-img1.png',
      title: 'PIL.title_1',
      description: `PIL.desc_1`,
      tokenSymbol: 'DAO'
    },
    {
      img: '/assets/media/mainpage/pillar-img2.png',
      title: 'PIL.title_2',
      description: `PIL.desc_2`,
      tokenSymbol: 'IDO'
    },
    {
      img: '/assets/media/mainpage/pillar-img3.png',
      title: 'PIL.title_3',
      description: `PIL.desc_3`,
      tokenSymbol: 'DEX'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
