import { Component, Input, OnInit } from '@angular/core';
import { interval } from 'rxjs';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-project-counter',
  templateUrl: './project-counter.component.html',
  styleUrls: ['./project-counter.component.scss']
})
export class ProjectCounterComponent implements OnInit {

  private subs: SubSink = new SubSink();

  @Input() startDate: Date;
  @Input() endDate: Date;
  @Input() statusProject: string | undefined = '';;

  secondsToDday = 0;
  minutesToDday = 0;
  hoursToDday = 0;
  daysToDday = 0;

  constructor() { }

  ngOnInit(): void {

    this.getTimeDifference();
    this.getShowCounter();
  }


  private getTimeDifference(): void {

    if (this.startDate && this.startDate !== null) {

      let timeDifference = this.startDate.getTime() - new Date().getTime();

      if (timeDifference > 0) {

        timeDifference = timeDifference / 1000;

        const hoursInADay = 24;
        const minutesInAnHour = 60;
        const secondsInAMinute = 60;

        this.secondsToDday = Math.floor((timeDifference) % secondsInAMinute);
        this.minutesToDday = Math.floor((timeDifference) / (minutesInAnHour) % secondsInAMinute);
        this.hoursToDday = Math.floor((timeDifference) / (minutesInAnHour * secondsInAMinute) % hoursInADay);
        this.daysToDday = Math.floor((timeDifference) / (minutesInAnHour * secondsInAMinute * hoursInADay));
      }

    }
  }

  private getShowCounter(): void {
    this.subs.sink = interval(1000).subscribe(x => { this.getTimeDifference(); });
  }
}
