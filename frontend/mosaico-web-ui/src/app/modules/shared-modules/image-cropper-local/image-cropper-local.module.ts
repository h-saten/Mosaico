import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImageCropperLocalComponent } from './image-cropper-local.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { ImageCropperLocalService } from './image-cropper-local.service';
import { TranslateModule } from '@ngx-translate/core';
import { TranslationService } from 'mosaico-base';
import { locale as enLangEn } from './i18n/en';
import { locale as enLangPl } from './i18n/pl';

@NgModule({
  declarations: [
    ImageCropperLocalComponent
  ],
  imports: [
    CommonModule,
    ImageCropperModule,
    TranslateModule
  ],
  exports: [ImageCropperLocalComponent],
  providers: [ImageCropperLocalService]
})
export class ImageCropperLocalModule {
  constructor(translateService: TranslationService) {
    translateService.loadTranslations(enLangEn, enLangPl);
  }

}
