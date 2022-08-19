import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ExternalProviderLoginService} from '../../../services/external-provider-login.service';

@Component({
  selector: 'app-kanga-login-redirect',
  templateUrl: './kanga-login-redirect.component.html',
  styleUrls: ['./kanga-login-redirect.component.scss']
})
export class KangaLoginRedirectComponent implements OnInit {

  loginError = false;

  constructor(
    private route: ActivatedRoute,
    private externalProviderLoginService: ExternalProviderLoginService
  ) {}

  ngOnInit(): void {
    this.externalProviderLoginService.KangaLogin();
  }

}
