import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { StonlyService } from './stonly.service';

@Directive({
    selector: '[stonly]'
})
export class StonlyDirective {
    constructor(private element: ElementRef, private stonlyService: StonlyService) {
    }

    @Input() code = '';
    @Input() isFull = false;

    @HostListener('click') onClick() {
        if(this.isFull === true) {
            this.stonlyService.openGuideFull(this.code);
        }
        else {
            this.stonlyService.openGuide(this.code);
        }
    }
}