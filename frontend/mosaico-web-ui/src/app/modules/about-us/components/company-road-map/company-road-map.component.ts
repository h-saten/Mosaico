import { Component, OnInit } from '@angular/core';

import { RoadMapItem } from './../models/roadmap-item';

@Component({
  selector: 'app-company-road-map',
  templateUrl: './company-road-map.component.html',
  styleUrls: ['./company-road-map.component.scss']
})
export class CompanyRoadMapComponent implements OnInit {
  panelOpenState: boolean = false;
  currentYear: number = 2022;

  roadMaps: RoadMapItem[] = [
    {
      year: 2022,
      quarters: [
        {
          title: 'Q1',
          steps: [
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_1',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_2',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_3',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_4',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_5',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_6',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_7',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_8',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_9',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_10',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_11',
            'ROADMAP.CURRENT_YEAR.QUARTER_1.STEP_12'
          ]
        },
        {
          title: 'Q2',
          steps: [
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_1',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_2',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_3',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_4',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_5',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_6',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_7',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_8',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_9',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_10',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_11',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_12',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_13',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_14',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_15',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_16',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_17',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_18',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_19',
            'ROADMAP.CURRENT_YEAR.QUARTER_2.STEP_20'
          ]
        },
        {
          title: 'Q3',
          steps: [
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_1',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_2',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_3',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_4',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_5',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_6',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_7',
            'ROADMAP.CURRENT_YEAR.QUARTER_3.STEP_8'
          ]
        },
        {
          title: 'Q4',
          steps: [
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_1',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_2',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_3',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_4',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_5',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_6',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_7',
            'ROADMAP.CURRENT_YEAR.QUARTER_4.STEP_8'
          ]
        }
      ]
    },
    {
      year: 2023,
      quarters: [
        {
          title: 'Q1',
          steps: [
            'ROADMAP.NEXT_YEAR.QUARTER_1.STEP_1',
            'ROADMAP.NEXT_YEAR.QUARTER_1.STEP_2'
          ]
        },
        {
          title: 'Q2',
        },
        {
          title: 'Q3',
        },
        {
          title: 'Q4',
        }
      ]
    },
    {
      year: 2024,
      quarters: [
        {
          title: 'Q1',
        },
        {
          title: 'Q2',
        },
        {
          title: 'Q3',
        },
        {
          title: 'Q4',
        }
      ]
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

  changeYear(year: number) {
    this.currentYear = year;
  }

}
