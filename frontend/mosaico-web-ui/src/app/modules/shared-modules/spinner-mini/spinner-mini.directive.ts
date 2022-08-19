import { Directive, Input, ElementRef, Renderer2, OnInit, OnChanges } from '@angular/core';

@Directive({
  selector: '[appSpinnerMini]'
})
export class SpinnerMiniDirective implements OnInit, OnChanges{

  @Input() showSpinnerMini: boolean;
  @Input() leftMargin: 1 | 2 | 3 | 4 | 5 = 2;

  private elem: HTMLElement;
  private elemSpinner: HTMLElement;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) { }

  ngOnInit(): void {
    this.elem = this.el.nativeElement;
  }

  ngOnChanges(): void {
    this.elem = this.el.nativeElement;
    if (this.showSpinnerMini === true) {
      this.showSpinner();

    } else {
      this.hideSpinner();
    }
  }

  showSpinner (): void {
    this.elemSpinner = this.renderer.createElement('span');
    const classes = `fas fa-spinner fa-spin text-white font-weight-bold ms-${this.leftMargin}`;
    this.renderer.setAttribute(this.elemSpinner, 'class', classes);
    this.renderer.appendChild(this.elem, this.elemSpinner);
  }

  hideSpinner (): void
  {
    if (this.elemSpinner) {
      const faSpinner = this.elem.querySelector('.fa-spinner') as HTMLElement;

      if (faSpinner) {
        faSpinner.remove();
      }

      this.renderer.removeChild(this.elem, this.elemSpinner);
    }
  }

}
