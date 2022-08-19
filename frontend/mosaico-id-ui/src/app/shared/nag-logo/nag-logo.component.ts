import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AppConfigurationService} from "../../services/app-configuration.service";

@Component({
  selector: 'app-nag-logo',
  templateUrl: './nag-logo.component.html',
  styleUrls: ['./nag-logo.component.scss']
})
export class NagLogoComponent implements OnInit {

  public urlReturn: string;

  constructor(
    private router: Router,
    private appConfigurationService: AppConfigurationService
    ) {}

  ngOnInit() {
    this.urlReturn = this.appConfigurationService.mosaicoAppUrl();
  }
}
