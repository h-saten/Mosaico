import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InlineSVGModule } from 'ngx-svg-inline';
import { InviteUsersModalComponent } from './invite-users-modal/invite-users-modal.component';
import { MainModalComponent } from './main-modal/main-modal.component';
import { ConfirmWalletDisconnectModal } from './';

@NgModule({
  declarations: [
    InviteUsersModalComponent,
    MainModalComponent,
    ConfirmWalletDisconnectModal
  ],
  imports: [CommonModule, InlineSVGModule, RouterModule],
  exports: [
    InviteUsersModalComponent,
    MainModalComponent,
    ConfirmWalletDisconnectModal
  ],
})
export class ModalsModule {}
