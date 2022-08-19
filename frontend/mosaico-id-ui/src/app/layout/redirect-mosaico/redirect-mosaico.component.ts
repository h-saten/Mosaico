import {Component, Inject, OnInit} from '@angular/core';
import {DOCUMENT} from '@angular/common';
import {AppConfigurationService} from "../../services/app-configuration.service";

@Component({
  selector: 'app-redirect-mosaico',
  template: '',
})
export class RedirectMosaicoComponent implements OnInit {

  constructor(
    @Inject(DOCUMENT) public document: Document,
    private appConfigurationService: AppConfigurationService
  ) {}

  ngOnInit() {
    return this.document.location.href = this.appConfigurationService.mosaicoAppUrl();
  }

}
