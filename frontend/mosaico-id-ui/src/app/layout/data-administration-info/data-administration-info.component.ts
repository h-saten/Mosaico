import {Component} from '@angular/core';
import {animate, state, style, transition, trigger} from '@angular/animations';

export enum States {
  FadeIn = 'fadeIn',
  FadeOut = 'fadeOut',
  Void = 'void',
}

@Component({
  selector: 'app-data-administration-info',
  templateUrl: './data-administration-info.component.html',
  styleUrls: ['./data-administration-info.component.scss'],
  animations: [
    trigger('toggle', [
      state(States.FadeIn, style({
        height: 0,
        opacity: 0
      })),
      state(States.FadeOut, style({
        height: 'auto',
        opacity: 1
      })),
      transition('fadeIn => fadeOut', [
        animate('0.5s')
      ]),
      transition('fadeOut => fadeIn', [
        animate('0.5s')
      ]),
    ]),
  ],
})
export class DataAdministrationInfoComponent {

  public toggleVisible: boolean;
  public toggleVisibleDesc = 'fadeIn';

  constructor() {
    this.toggleVisible = false;
    this.toggleVisibleDesc = States.FadeIn;
  }


  toggleAdministrationInfo() {
    this.toggleVisible = !this.toggleVisible;
    this.toggleVisibleDesc = this.toggleVisibleDesc === States.FadeIn ? States.FadeOut : States.FadeIn;
  }

}
