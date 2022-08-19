import { AfterViewInit, Directive, ElementRef, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[appProjectTitle]'
})
export class ProjectTitleDirective implements AfterViewInit {

  @Output() numberWidthEvent: EventEmitter<number> = new EventEmitter<number>();

  private title: HTMLElement;

  constructor(
    private el: ElementRef,
  ) { }

  ngAfterViewInit(): void {

    this.title = this.el.nativeElement as HTMLElement;

    setTimeout(() => {
      this.readTheWidthOfTheElements();
    }, 300);
  }

  @HostListener('window:resize')
  private onResize(): void  {
    this.readTheWidthOfTheElements();
  }

  private readTheWidthOfTheElements (): void {

    if (this.title) {
      const titleWidth: number = this.title.offsetWidth;
      if (titleWidth > 0) {
        this.numberWidthEvent.emit(titleWidth);
      }
    }
  }

}
