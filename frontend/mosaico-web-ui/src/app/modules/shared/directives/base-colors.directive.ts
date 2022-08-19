import { Directive, ElementRef, HostListener, Renderer2, HostBinding, Input, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Page } from 'mosaico-project';
import { SubSink } from 'subsink';
import { selectProjectPage } from '../../project-management/store';

@Directive({
  selector: '[appBaseColors]'
})
export class BaseColorsDirective implements OnInit, AfterViewInit, OnDestroy {

  private defaultPrimaryColor = '#0063F5';
  private defaultPrimaryColorMenu = 'linear-gradient(90deg, #0063F5 0%, #00A1FF 100%)';
  private primaryColor = '';
  private secondaryColor = '';
  private defaultSecondaryColor = '#ffffff';
  private coverColor = '';

  @Input() whichColor: string;
  @Input() textColor?: boolean;
  @Input() pulse?: boolean;

  @Input() button?: boolean;
  @Input() border?: boolean;
  @Input() isHighLightedBg: boolean;

  @Input() menu?: boolean;
  @Input() isSvg?: boolean;
  @Input() iconSvg?: boolean;
  @Input() iconSvgCover?: boolean;
  @Input() iconAwesome?: boolean;
  @Input() headerTable?: boolean;
  @Input() progressBar?: boolean;
  @Input() switch?: boolean;
  @Input() elHeader?: boolean;

  private elem: HTMLElement = null;
  private elemInput: HTMLInputElement = null;

  private colorOfButton = '';
  private colorOfButtonHover = '';
  private colorOfHeaderTable = '';

  private subs = new SubSink();
  private page: Page;

  private intervalSvgElem: NodeJS.Timer;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    private store: Store,
  ) {
    this.elem = this.el.nativeElement;
    this.elemInput = this.el.nativeElement;
  }

  ngOnInit(): void {

    this.getColors();
  }

  ngAfterViewInit(): void {

    if ((this.iconSvg === true || this.iconSvgCover === true) && this.elem && this.colorOfButton) {
      this.intervalSvgElem = setInterval(() => {
        const svgElem = this.elem.children[0];
        if (svgElem) {
          this.setFillForSvg();
          if(this.intervalSvgElem){
            clearInterval(this.intervalSvgElem);
          }
        }
      }, 100);

    }
  }

  ngOnDestroy(): void {
    if (this.intervalSvgElem) {
      clearInterval(this.intervalSvgElem);
    }
  }

  private getColors(): void {
    // just for the project

    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res) {
        this.page = res;
        this.primaryColor = this.page.primaryColor;
        this.secondaryColor = this.page.secondaryColor;
        this.coverColor = this.page.coverColor;

        if (this.primaryColor) {
          this.colorOfButton = this.primaryColor;

          if (this.headerTable === true) {
            // this.colorOfHeaderTable = this.lightenDarkenColor(this.primaryColor, -48);
          }

          this.colorOfButtonHover = this.lightenDarkenColor(this.primaryColor, -15);

          // this won't work with refreshing - because icon fontawesome - won't generate an internal element yet -
          // - but it will work with color change
          if (this.iconSvg === true || this.iconSvgCover === true) {
            this.setFillForSvg();
          }

        }
      }
    });
  }

  private setFillForSvg (where?: string): void {
    if (this.iconSvg === true && this.elem && this.primaryColor) {
      if (this.elem.classList.contains('svg-icon') === true) {
        const svgElem = this.elem.children[0];
        if (svgElem) {
          const pathElem: HTMLCollection = svgElem.children;
          if (pathElem) {
            // HTMLCollection
            for (const key in pathElem) {
              if (pathElem.hasOwnProperty(key)) {
                this.renderer.setStyle(pathElem[key], 'fill', this.primaryColor);
              }
            }
          }
        }
      }
    }


    if (this.iconSvgCover === true && this.elem && this.coverColor) {
      if (this.elem.classList.contains('svg-icon') === true) {
        const svgElem = this.elem.children[0];
        if (svgElem) {
          const pathElem: HTMLCollection = svgElem.children;
          if (pathElem) {
            // HTMLCollection
            for (const key in pathElem) {
              if (pathElem.hasOwnProperty(key)) {
                this.renderer.setStyle(pathElem[key], 'fill', this.coverColor);
              }
            }
          }
        }
      }
    }
  }

  @HostBinding('style.color')
  get cssStyleColor(): string {

    if (this.primaryColor) {
      if (this.textColor === true || this.isSvg === true) {
        return this.primaryColor;
      }
    }

    if (this.secondaryColor) {
      if (this.button === true || this.headerTable === true || this.iconAwesome === true) {
        return this.secondaryColor;

      } else if (this.menu === true) {
        if (this.elem.classList.contains('active')) {
          return this.secondaryColor;
        }
      }

    } else {
      if (this.menu === true) {
        if (this.elem.classList.contains('active') === false) {
          return '#494642';
        }
      }
    }

    if (this.coverColor) {
      if (this.elHeader === true) {
        return this.coverColor;
      }
    }
  }

  @HostBinding('style.background-color')
  get cssStyleBackgroundColor(): string {

    if (this.primaryColor) {
      if (this.button ===  true
        || this.pulse === true
        || this.iconAwesome === true
        || this.progressBar === true
      ) {
        return this.primaryColor;

      } else if (this.menu === true) {
        if (this.elem.classList.contains('active')) {
          return this.primaryColor;
        }

      } else if (this.headerTable === true) {
        if (this.colorOfHeaderTable) {
          return this.colorOfHeaderTable;
        } else {
          return this.primaryColor;
        }

      } else if (this.switch === true) {
        if (this.elemInput.checked === true) {
          return this.primaryColor;
        }
      }

      if(this.isHighLightedBg === true) {
        let a = this.hexToRgb(this.primaryColor);
        a.push(0.2);
        return `rgba(${a.toString()})`;
      }
    }
  }

  @HostBinding('style.background-image')
  get cssStyleBackground(): string {

    if (this.primaryColor) {
      // for the active item at the first visit to the project website
      if (this.menu === true) {
        if (this.elem.classList.contains('active')) {
          return 'inherit';
        }
      }

    } else {
      if (this.menu === true) {
        if (this.elem.classList.contains('active') === false) {
          return 'inherit';
        }
      }
    }
  }

  // this is related to .btn-menu:hover
  // @HostBinding('style.--hover-background')
  // get cssStyleBackgroundHover(): string {

  //   if (this.menu === true) {
  //     if (this.primaryColor) {
  //         return 'transparent';
  //     } else {
  //       return 'linear-gradient(90deg, #0063F5 0%, #00A1FF 100%)';
  //     }
  //   }
  // }

  @HostBinding('style.border-color')
  get cssStyleBorderColor(): string {

    if (this.primaryColor) {
      if (this.button ===  true || this.pulse === true) {
        return this.primaryColor;
      }
      if(this.border === true) {
        return this.primaryColor;
      }
    }
  }

  @HostBinding('style.box-shadow')
  get cssStyleBoxShadowColor(): string {

    if (this.primaryColor) {
      if (this.pulse === true) {
        return '0 0 0 0 ' + this.primaryColor;
      }
    }
  }


  @HostListener('mouseenter')
  onMouseEnter(): void {
    // onMouseEnter($event: Event): void {

    if (this.button === true || this.menu === true) {

      if (this.primaryColor) {
        if (this.button === true) {
          this.renderer.setStyle(this.elem, 'backgroundColor', this.primaryColor, 1);
          this.renderer.setStyle(this.elem, 'borderColor', this.primaryColor, 1);

        } else if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.setStyle(this.elem, 'backgroundColor', this.primaryColor);
            this.renderer.setStyle(this.elem, 'borderColor', this.primaryColor);
          }
        }
        // this.renderer.setAttribute(this.elemI, 'class', 'far fa-copy fa-lg');

      } else {
        if (this.button === true) {
          this.renderer.setStyle(this.elem, 'backgroundColor', this.defaultPrimaryColor, 1);
          this.renderer.setStyle(this.elem, 'borderColor', this.defaultPrimaryColor, 1);

        } else if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.setStyle(this.elem, 'backgroundImage', this.defaultPrimaryColorMenu);
            // this.renderer.setStyle(this.elem, 'borderColor', this.defaultPrimaryColorMenu);
          }
        }
      }


      if (this.secondaryColor) {
        if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.setStyle(this.elem, 'color', this.secondaryColor);
          }
        }
      } else {
        if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.setStyle(this.elem, 'color', this.defaultSecondaryColor);
          }
        }
      }

    } else {
      // if (this.button === true) {
      //   this.renderer.setStyle(this.elem, 'backgroundColor', this.defaultPrimaryColor, 1);
      //   this.renderer.setStyle(this.elem, 'borderColor', this.defaultPrimaryColor, 1);
      // }
    }
  }

  @HostListener('mouseleave')
  onMouseLeave(): void {

    if (this.button === true || this.menu === true) {

      if (this.primaryColor) {
        if (this.button === true || this.pulse === true) {
          this.renderer.setStyle(this.elem, 'backgroundColor', this.primaryColor, 1);
          this.renderer.setStyle(this.elem, 'borderColor', this.primaryColor, 1);

        } else if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.removeStyle(this.elem, 'backgroundColor');
            this.renderer.removeStyle(this.elem, 'borderColor');
          }
        }

      } else {

        if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.removeStyle(this.elem, 'backgroundImage');
            // this.renderer.removeStyle(this.elem, 'borderColor');
          }
        }
      }

      if (this.secondaryColor) {
        if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.removeStyle(this.elem, 'color');
          }
        }

      } else {
        if (this.menu === true) {
          if (this.elem.classList.contains('active') === false) {
            this.renderer.removeStyle(this.elem, 'color');
          }
        }
      }


    }

  }

  lightenDarkenColor(color: string, amt: number): string {

    // https://css-tricks.com/snippets/javascript/lighten-darken-color/

    let useHash = false;

    if (color[0] === '#') {
      color = color.slice(1);
      useHash = true;
    }

    const num = parseInt(color, 16);

    // tslint:disable-next-line: no-bitwise
    let r = (num >> 16) + amt;

    if (r > 255) { r = 255; }
    else if (r < 0) { r = 0; }

    // tslint:disable-next-line: no-bitwise
    let b = ((num >> 8) & 0x00FF) + amt;

    if (b > 255) { b = 255; }
    else if (b < 0) { b = 0; }

    // tslint:disable-next-line: no-bitwise
    let g = (num & 0x0000FF) + amt;

    if (g > 255) { g = 255; }
    else if (g < 0) { g = 0; }

    // tslint:disable-next-line: no-bitwise
    return (useHash ? '#' : '') + (g | (b << 8) | (r << 16)).toString(16);
  }

  hexToRgb(hex: string) {
    // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
    const shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
    hex = hex.replace(shorthandRegex, (m, r, g, b) => {
      return r + r + g + g + b + b;
    });
  
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result
      ? [
        parseInt(result[1], 16),
        parseInt(result[2], 16),
        parseInt(result[3], 16)
      ]
      : null;
  }
}