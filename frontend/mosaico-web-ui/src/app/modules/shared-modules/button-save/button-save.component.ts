import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-button-save',
  templateUrl: './button-save.component.html',
  styleUrls: ['./button-save.component.scss']
})
export class ButtonSaveComponent {
  @Input() classes = 'btn btn-primary btn-click';
  @Input() buttonId?: string;
  @Input() formInvalid?: boolean;
  @Input() showSpinner: boolean;
  @Input() projectColor?: boolean;
  @Input() buttonText?: string;
  @Input() imgUrl?: string;
  @Output() clicked = new EventEmitter<any>();

  constructor() { }

  onClick(): void {
    if(!this.formInvalid && !this.showSpinner){
      this.clicked.emit();
    }
  }

}
