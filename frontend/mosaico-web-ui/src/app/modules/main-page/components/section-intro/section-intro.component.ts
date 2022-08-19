import { Component, OnInit } from '@angular/core';
import { CounterService, GetKPIResponse } from 'mosaico-base';

@Component({
  selector: 'app-section-intro',
  templateUrl: './section-intro.component.html',
  styleUrls: ['./section-intro.component.scss']
})
export class SectionIntroComponent implements OnInit {

  constructor(private counterService: CounterService) { }
  counters: GetKPIResponse;

  async ngOnInit(): Promise<void> {
    try{
      const counterResponse = await this.counterService.getKPIs().toPromise();
      this.counters = counterResponse?.data;
    }
    catch(e) {
      //ignore
      this.counters = null;
    }
  }

}
