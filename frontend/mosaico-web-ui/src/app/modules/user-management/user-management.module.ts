import { InlineSVGModule } from 'ngx-svg-inline';
import { UserManagementRoutingModule } from './user-management-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KYCPage, ProfileComponent, UserManagementRootPage } from './pages';
import { EditProfileComponent, ProfileDetailsComponent } from './components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalProfileComponent } from './components/modal-profile/modal-profile.component';
import { NgbActiveModal, NgbModalModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared';
import { ModalChangeEmailComponent } from './components/modal-change-email/modal-change-email.component';
import { ModalChangePasswordComponent } from './components/modal-change-password/modal-change-password.component';
import { PasswordConfirmAccountDeleteComponent } from './modals/password-confirm-account-delete/password-confirm-account-delete.component';
import { locale as enLang } from './i18n/en';
import { locale as plLang } from './i18n/pl';
import { VerifyPhoneNumberComponent } from "./components/verify-phone-number/verify-phone-number.component";
import { PhoneNumberFormComponent } from "./components/verify-phone-number/phone-number-form/phone-number-form.component";
import { ConfirmationCodeFormComponent } from "./components/verify-phone-number/confirmation-code-form/confirmation-code-form.component";
import {
  ImageCropperLocalModule,
  ButtonSaveModule,
  SpinnerMiniModule
} from "../shared-modules";
import { MosaicoBaseModule, TranslationService } from 'mosaico-base';
import { NgSelectModule } from '@ng-select/ng-select';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EvaluationFormComponent } from './pages/evaluation-form/evaluation-form.component';

@NgModule({
  declarations: [
    KYCPage,
    ProfileComponent,
    UserManagementRootPage,
    ProfileDetailsComponent,
    EditProfileComponent,
    ModalProfileComponent,
    ModalChangeEmailComponent,
    ModalChangePasswordComponent,
    PasswordConfirmAccountDeleteComponent,
    VerifyPhoneNumberComponent,
    PhoneNumberFormComponent,
    ConfirmationCodeFormComponent,
    EvaluationFormComponent
  ],
  imports: [
    CommonModule,
    UserManagementRoutingModule,
    InlineSVGModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    NgbTooltipModule,
    ImageCropperLocalModule,
    ButtonSaveModule,
    SpinnerMiniModule,
    NgSelectModule,
    NgbModule,
    MosaicoBaseModule
  ],
  entryComponents: [
    ModalChangeEmailComponent
  ],
  providers: [
    NgbActiveModal
  ]
})
export class UserManagementModule {
  constructor(translationService: TranslationService) {
    translationService.loadTranslations(enLang, plLang);
  }
}
