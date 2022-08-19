import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { fromEvent } from 'rxjs';
import { pairwise, switchMap, takeUntil, tap } from 'rxjs/operators';

@Directive({
  selector: '[appMatTabTouch]'
})
export class MatTabTouchDirective implements OnInit {

  private mattabHeader: HTMLDivElement;
  private mattabList: HTMLDivElement;
  private originalMattabListTransition?: string;
  private headersMaxScroll?: number;

  private mattabBodyWrapper: HTMLDivElement;
  private skipBodySwipe = false;

  private bodyCurrentScroll?: {
    x: number;
    y: number
  };

  @Input() swipeLimitWidth = 80; // to dot przesuwania body
  @Input() connectEdges = true; // chyba to usunę
  // jeśli jest false - to po naciśnięciu nie przesuwa częściowo pokazywanych zakładek - aby się cała pokazała
  // idealnie powinno być tak że po naciśnieciu zakładki - pozycjonowała się na środku - jeśli po lewej i prawej ma zakładki

  private centerTab = false;

  constructor(
    private tabGroup: MatTabGroup,
    private el: ElementRef,
  ) { }

  ngOnInit(): void {

    const elem: HTMLElement = this.el.nativeElement;
    const El: HTMLElement = this.tabGroup._elementRef.nativeElement;

    if (elem) {
      this.mattabHeader = elem.querySelector<HTMLDivElement>('mat-tab-header');
      this.mattabList = this.mattabHeader.querySelector<HTMLDivElement>('.mat-tab-list');
      // console.log('this.mattabList: ', this.mattabList);

      this.mattabBodyWrapper = elem.querySelector<HTMLDivElement>('.mat-tab-body-wrapper');
      // console.log('this.mattabBodyWrapper: ', this.mattabBodyWrapper);
    }

    if (!this.mattabHeader && !this.mattabList && !this.mattabBodyWrapper) {
      return;
    }

    this._handleHeadersEvents();

    // this._handleBodyEvents(); // to do przesuwania w lewo i prawo zakładki poprzez naciśnięcie na głownej treści body zakładki
    // narazie nie działa
  }

  private _handleHeadersEvents(): void {

    // console.log('this.mattabHeader: ', this.mattabHeader);

    // this will capture all touchstart events from the mattabHeader element
    fromEvent(this.mattabHeader, 'touchstart')
      .pipe(
        tap(() => {

          this.originalMattabListTransition = this.mattabList.style.transition;
          this.mattabList.style.transition = 'none';
          this.headersMaxScroll = -1 * (this.mattabList.offsetWidth - this.mattabHeader.offsetWidth + 64);

        }),
        switchMap((e) => {

          // console.log('e: ', e);

          // after a mouse down, we'll record all mouse moves
          return fromEvent(this.mattabHeader, 'touchmove')
            .pipe(
              // we'll stop (and unsubscribe) once the user releases the mouse
              // this will trigger a 'mouseup' event

              takeUntil(
                fromEvent(this.mattabHeader, 'touchend')
                .pipe(
                  tap(() => {
                      this.mattabList.style.transition = this.originalMattabListTransition;
                      this.centerTab = true;
                    })
                )
              ),
              // pairwise lets us get the previous value to draw a line from
              // the previous point to the current point
              pairwise()
            );
        })
      )
      .subscribe((res: [any, any]) => {

        const rect = this.mattabHeader.getBoundingClientRect();
        // previous and current position with the offset
        const prevX = res[0].touches[0].clientX - rect.left;
        const currentX = res[1].touches[0].clientX - rect.left;
        this._scrollHeaders( currentX - prevX);
      });

  }

  private _scrollHeaders(scrollX: number): void {

    // console.log('scrollX: ', scrollX);

    if (!this.mattabList || !this.headersMaxScroll) {
      return;
    }

    const currentTransform = this.mattabList.style.transform;

    // console.log('currentTransform: ', currentTransform);

    let currentScroll: number;

    if (currentTransform && currentTransform.indexOf('translateX') > -1) {
      let tmp = currentTransform.substring('translateX('.length);
      tmp = tmp.substring(0, tmp.length - 'px)'.length);
      currentScroll = parseInt(tmp, 10);

    } else {
      currentScroll = 0;
    }

    let newScroll = currentScroll + scrollX;
    if (newScroll > 0) {
      newScroll = 0;
    }
    if (newScroll < this.headersMaxScroll) {
      newScroll = this.headersMaxScroll;
    }

    this.mattabList.style.transform = `translateX(${newScroll}px)`;
  }
}

// https://github.com/Gaiidenn/angular-material-gesture
// https://www.npmjs.com/package/@angular-material-gesture/mat-tab-group-gesture
// https://stackblitz.com/edit/mat-tab-group-gesture-demo?file=src/app/app.component.ts (edited)
