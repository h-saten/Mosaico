import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from '../../../shared/shared.module';
import {KangaLoginComponent} from './kanga-login.component';
import {KangaLoginRoutingModule} from './kanga-login-routing.module';

@NgModule({
  imports: [
    CommonModule,
    KangaLoginRoutingModule,
    SharedModule
  ],
  declarations: [KangaLoginComponent]
})
export class KangaLoginModule { }
