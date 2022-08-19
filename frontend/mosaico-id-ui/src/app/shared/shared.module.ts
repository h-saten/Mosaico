import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardComponent } from './card/card.component';
import { CardToggleDirective } from './card/card-toggle.directive';
import { SpinnerComponent } from './spinner/spinner.component';

import { TranslateModule, TranslatePipe } from '@ngx-translate/core';
import { LanguageSelectorComponent } from '../shared/language-selector/language-selector.component';
import { NagLogoComponent } from '../shared/nag-logo/nag-logo.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { Routes, RouterModule } from '@angular/router'; // tutaj dodane aby komponent nag-logo widział routerlinki

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,

    RouterModule
  ],
  exports: [
    CardToggleDirective,
    CardComponent,
    SpinnerComponent,
    TranslatePipe,
    LanguageSelectorComponent,
    NagLogoComponent,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  declarations: [
    CardToggleDirective,
    CardComponent,
    SpinnerComponent,
    LanguageSelectorComponent,
    NagLogoComponent
  ]
})
export class SharedModule { }
