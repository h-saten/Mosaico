import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { LanguageEnum } from '../../models';

@Component({
  selector: 'lib-language-picker',
  templateUrl: './language-picker.component.html',
  styleUrls: ['./language-picker.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LanguagePickerComponent implements OnInit, OnChanges {
  languages = LanguageEnum;
  currentDisplayLanguage: string;
  @Input() currentLang: string = LanguageEnum.EN;
  @Input() styles: string;
  @Output() languageChanged: EventEmitter<string> = new EventEmitter<string>();

  constructor() { }
  
  ngOnChanges(changes: SimpleChanges): void {
    this.setDisplayLanguage();
  }

  ngOnInit(): void {
    
  }

  private setDisplayLanguage(){
    this.currentDisplayLanguage = Object.keys(LanguageEnum).find((l) => this.languages[l] === this.currentLang);
  }
  
  selectLang(langValue: string): void {
    if(langValue && langValue.length > 0 && langValue !== this.currentLang) {
      this.currentLang = langValue;
      this.languageChanged.emit(langValue);
      this.setDisplayLanguage();
    }
  }
}
