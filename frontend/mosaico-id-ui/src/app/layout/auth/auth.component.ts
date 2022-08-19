import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {AppConfigurationService} from "../../services/app-configuration.service";

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss'],
})
export class AuthComponent implements OnInit {

  public namePath_0: string; // potrzebne title
  public title;

  public addClass: string;
  public dataAdministrationInfo: boolean;
  public urlReturn: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private appConfigurationService: AppConfigurationService
  ) {
    this.dataAdministrationInfo = false;
    this.urlReturn = this.appConfigurationService.appUrl();
  }

  ngOnInit() {

    this.title = this.activatedRoute.snapshot.data.title;

    const administrationInfo = this.activatedRoute.snapshot.data.data_administration_info;

    this.dataAdministrationInfo = administrationInfo !== undefined && administrationInfo !== false;

    this.namePath_0 = this.activatedRoute.snapshot.data.path_0;

    if (this.namePath_0 === 'registration')
    {
      this.addClass = 'signup-card';
    } else
    {
      this.addClass = 'login-card';
    }

  }


}
