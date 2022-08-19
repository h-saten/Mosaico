import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-inline-edit',
  templateUrl: './inline-edit.component.html',
  styleUrls: ['./inline-edit.component.scss']
})
export class InlineEditComponent implements OnInit, AfterViewInit {
  @Input() data: string;
  @Input() textColor = "#000000";
  // @Input() height: string = "75px";
  @Output() focusOut: EventEmitter<string> = new EventEmitter<string>();
  @ViewChild('input') input: ElementRef;

  constructor() { }

  ngAfterViewInit(): void {
    if(this.input){
      this.input.nativeElement.focus();
    }
  }

  ngOnInit(): void {

  }

  onFocusOut(): void {
    this.focusOut.emit(this.data);
  }

  blur(e: any): void {
    if(e){
      const target = e.target as HTMLTextAreaElement;
      if(target){
        target.blur();
      }
    }
  }
}
