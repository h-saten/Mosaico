import { AfterContentInit, Directive, ElementRef, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[appMenuProjects]',
  exportAs: 'MPDirective'
})
export class MenuProjectsDirective implements AfterContentInit {

  @Output() numberHeightEvent: EventEmitter<number> = new EventEmitter<number>();

  private menuProjectsBtnMenu: HTMLElement;
  private menuProjectsMenu: HTMLElement;

  constructor(
    private el: ElementRef,
  ) { }

  ngAfterContentInit(): void {

    this.menuProjectsBtnMenu = this.el.nativeElement as HTMLElement;
    this.menuProjectsMenu = this.menuProjectsBtnMenu.parentElement;

    setTimeout(() => {
      this.readTheHeightOfTheElements();
    }, 100);
  }

  @HostListener('window:resize')
  private onResize(): void  {
    this.readTheHeightOfTheElements();
  }

  private readTheHeightOfTheElements (): void {

    if (this.menuProjectsBtnMenu) {
      const menuProjectsbtnMenuHeight: number = this.menuProjectsBtnMenu.offsetHeight;

      if (menuProjectsbtnMenuHeight > 0) {
        this.numberHeightEvent.emit(menuProjectsbtnMenuHeight);
      }
    }
  }
}
