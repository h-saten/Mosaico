import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../../../shared/shared.module';
import {KangaLoginRedirectComponent} from './kanga-login-redirect.component';
import {KangaLoginRedirectRoutingModule} from './kanga-login-redirect-routing.module';

@NgModule({
  imports: [
    CommonModule,
    KangaLoginRedirectRoutingModule,
    SharedModule
  ],
  declarations: [KangaLoginRedirectComponent]
})
export class KangaLoginModule { }
