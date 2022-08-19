import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InlineSVGModule } from 'ngx-svg-inline';
import { RouterModule, Routes } from '@angular/router';
import {
  NgbDropdownModule,
  NgbProgressbarModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { LayoutComponent } from './layout.component';
import { ExtrasModule } from '../partials/layout/extras/extras.module';
import { Routing } from '../../pages/routing';
import { AsideComponent } from './components/aside/aside.component';
import { HeaderComponent } from './components/header/header.component';
import { ContentComponent } from './components/content/content.component';
import { FooterComponent } from './components/footer/footer.component';
import { ScriptsInitComponent } from './components/scripts-init/scripts-init.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { AsideMenuComponent } from './components/aside/aside-menu/aside-menu.component';
import { TopbarComponent } from './components/topbar/topbar.component';
import { PageTitleComponent } from './components/header/page-title/page-title.component';
import { HeaderMenuComponent } from './components/header/header-menu/header-menu.component';
import { DrawersModule, DropdownMenusModule, ModalsModule } from '../partials';
import { KycAlertComponent } from './components';
import { InfoCookiesComponent } from './components/info-cookies/info-cookies.component';
import { SharedModule } from 'src/app/modules/shared';
import { RoleManagerModule } from '../../modules/role-manager/role-manager.module';
import { MosaicoBaseModule } from 'mosaico-base';
import { LanguageSwitchComponent } from './components/language-switch/language-switch.component';
import { FooterSocialIconComponent } from './components/footer/footer-social-icon/footer-social-icon.component';
import { FooterUrlDocumentsComponent } from './components/footer/footer-url-documents/footer-url-documents.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: Routing,
  },
];

@NgModule({
  declarations: [
    LayoutComponent,
    AsideComponent,
    HeaderComponent,
    ContentComponent,
    FooterComponent,
    ScriptsInitComponent,
    ToolbarComponent,
    AsideMenuComponent,
    TopbarComponent,
    PageTitleComponent,
    HeaderMenuComponent,
    KycAlertComponent,
    InfoCookiesComponent,
    LanguageSwitchComponent,
    FooterSocialIconComponent,
    FooterUrlDocumentsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MosaicoBaseModule,
    InlineSVGModule,
    NgbDropdownModule,
    NgbProgressbarModule,
    ExtrasModule,
    ModalsModule,
    DrawersModule,
    DropdownMenusModule,
    NgbTooltipModule,
    TranslateModule,
    SharedModule,
    RoleManagerModule
  ],
  exports: [RouterModule],
})
export class LayoutModule {}
