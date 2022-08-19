import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {KangaClient} from '../../../services/tokenizer-auth-api.service';
import {DOCUMENT} from '@angular/common';
import {AppConfigurationService} from "../../../services/app-configuration.service";

@Component({
  selector: 'app-kanga-login',
  templateUrl: './kanga-login.component.html',
  styleUrls: ['./kanga-login.component.scss']
})
export class KangaLoginComponent implements OnInit {

  loginError = false;

  constructor(
    private route: ActivatedRoute,
    private kangaClient: KangaClient,
    @Inject(DOCUMENT) private document: Document,
    private appConfigurationService: AppConfigurationService
  ) {}

  ngOnInit() {
    const token = this.route.snapshot.params.token;

    this.kangaClient.login(token).subscribe(response => {
      return this.document.location.href = this.appConfigurationService.mosaicoAppUrl();
    }, err => {
      this.loginError = true;
    });
  }

}
