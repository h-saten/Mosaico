import { Component, OnInit } from '@angular/core';
import { ControlValueAccessor, FormControl, FormGroup, NG_VALUE_ACCESSOR} from '@angular/forms';

@Component({
  selector: 'app-token-type-selector',
  templateUrl: './token-type-selector.component.html',
  styleUrls: ['./token-type-selector.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: TokenTypeSelectorComponent
    }
  ]
})
export class TokenTypeSelectorComponent implements OnInit, ControlValueAccessor   {
  form: FormGroup;
  typeControl: FormControl;
  onChange = (type) => {};
  onTouched = () => {};

  constructor() { }

  writeValue(type: string): void {
    this.typeControl.setValue(type);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
     if(isDisabled){
       this.form.disable();
     }
     else{
       this.form.enable();
     }
  }

  ngOnInit(): void {
    this.typeControl = new FormControl(null);
    this.form = new FormGroup({
      type: this.typeControl
    });
  }

}
