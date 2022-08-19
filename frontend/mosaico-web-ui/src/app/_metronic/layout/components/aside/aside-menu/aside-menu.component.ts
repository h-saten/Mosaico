import { Component, OnInit } from '@angular/core';
import { ConfigService } from 'mosaico-base';

@Component({
  selector: 'app-aside-menu',
  templateUrl: './aside-menu.component.html',
  styleUrls: ['./aside-menu.component.scss'],
})
export class AsideMenuComponent implements OnInit {
  appAngularVersion: string;

  constructor(configService: ConfigService) {
    this.appAngularVersion = configService.getConfig().appVersion;
  }

  ngOnInit(): void {}
}
