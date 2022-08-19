import { Component, OnInit, Input, OnDestroy } from '@angular/core';

// import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-spinner-container',
  templateUrl: './spinner-container.component.html',
  styleUrls: ['./spinner-container.component.scss']
})
export class SpinnerContainerComponent implements OnInit, OnDestroy {

  @Input() showSpinner: boolean;
  @Input() full?: boolean;
  @Input() type?: string;
  // public SpinnerContainerVisible: boolean;

  public nameId: string;

  constructor(
    // private spinner: NgxSpinnerService
    ) {

    // this.SpinnerContainerVisible = true;
    if (!this.type)
    {
      this.type = 'cube';
    }

  }

  ngOnInit() {
    // this.spinner.show();
    if (this.full === true)
    {
      this.nameId = 'http-loader';
    }
  }

  ngOnDestroy ()
  {
    // this.spinner.hide();
  }


}
