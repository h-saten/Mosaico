import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TranslationService } from 'mosaico-base';
import { UserInformation } from '../modules/user-management/models';

declare function stonlyTrack(propName: string, id: string, payload?: any);
declare function StonlyWidget(funcName: string, payload?: any);

@Injectable({
  providedIn: 'root'
})
export class StonlyService {

  constructor(private translationService: TranslateService) { }

  identify(user: UserInformation): void {
    if (user?.id && user?.id.length > 0) {
      stonlyTrack('identify', user.id, { 'email': user.email });
    }
  }

  setLanguage(lang: string): void {
    if (lang && lang.length > 0) {
      StonlyWidget('setWidgetLanguage', lang);
    }
  }

  openGuide(name: string): void {
    if (name && name.length > 0) {
      const lang = this.translationService.currentLang;
      if (lang && lang.length > 0) {
        this.setLanguage(lang);
      }
      StonlyWidget('openGuide', { guideId: name });
    }
  }

  openGuideFull(name: string): void {
    if (name && name.length > 0) {
      const lang = this.translationService.currentLang;
      if (lang && lang.length > 0) {
        this.setLanguage(lang);
      }
      StonlyWidget('openGuide', {
        guideId: name, widgetOptions: {
          widgetPlacement: 'MODAL',
          widgetFormat: 'LIGHT',
          widgetSizeType: 'FULL'
        }
      });
    }
  }

  close(): void {
    StonlyWidget('closeWidgets');
  }
}
