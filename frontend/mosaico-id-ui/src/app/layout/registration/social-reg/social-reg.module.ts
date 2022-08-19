import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SocialRegComponent} from './social-reg.component';
import {SocialRegRoutingModule} from './social-reg-routing.module';
import {SharedModule} from '../../../shared/shared.module';
import {CoreUserDataFormComponent} from "./core-user-data-form/core-user-data-form.component";
import {ReactiveFormsModule} from "@angular/forms";
import { RecaptchaV3Module } from "ng-recaptcha";

@NgModule({
  imports: [
    CommonModule,
    SocialRegRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    ReactiveFormsModule,
    ReactiveFormsModule,
    RecaptchaV3Module
  ],
  declarations: [
    SocialRegComponent,
    CoreUserDataFormComponent,
  ]
})
export class SocialRegModule { }
