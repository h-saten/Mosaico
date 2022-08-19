import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-color-edit',
  templateUrl: './color-edit.component.html',
  styleUrls: ['./color-edit.component.scss']
})
export class ColorEditComponent implements OnInit {

  @Input() displayColor: string;
  @Output() selectedColorEvent = new EventEmitter<string>();
  @Output() pickerOpenedEvent = new EventEmitter<boolean>();
  displayOldColor = '';
  displayNewColor = '';

  dataSavingRequestInProgress = false;
  isEditingIsInProgress = false;

  toggle = false;

  dialogDisplay = 'inline';

  constructor() { }

  ngOnInit(): void {
    this.displayOldColor = this.displayColor;
  }

  // to lock or hide buttons in a modal
  colorPickerOpenCloseEvent(event: string): void {
    if (event === 'colorPickerOpen') {
      this.toggle = true;
      this.pickerOpenedEvent.emit(true);
    } else if (event === 'colorPickerClose') {
      this.pickerOpenedEvent.emit(false);
      this.selectedColorEvent.emit(this.displayNewColor);
    }
  }

  onEventLog(event: string, data: string ): void {
    if (event === 'colorPickerSelect') {
      // this.selectedColorEvent.emit(data);
      this.displayNewColor = data;
    } else {
      // this.selectedColorEvent.emit(this.displayColor);
      this.displayNewColor = this.displayColor;
    }

    this.toggle = false;
    this.dialogDisplay = 'popup'; // to hide the color box because it is inline
  }

}
