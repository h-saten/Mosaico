import { Component, OnInit, Input, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-spinner-container',
  templateUrl: './spinner-container.component.html',
  styleUrls: ['./spinner-container.component.scss']
})
export class SpinnerContainerComponent implements OnInit, OnDestroy {

  @Input() showSpinner: boolean;
  @Input() full?: boolean;
  @Input() type?: string;

  public nameId: string;

  constructor(
    // private spinner: NgxSpinnerService
  ) {

    if (!this.type) {
      this.type = 'cube';
    }

  }

  ngOnInit(): void {
    // this.spinner.show();

    if (this.full === true) {
      this.nameId = 'http-loader';
    }
  }

  ngOnDestroy (): void {
    // this.spinner.hide();
  }

}
