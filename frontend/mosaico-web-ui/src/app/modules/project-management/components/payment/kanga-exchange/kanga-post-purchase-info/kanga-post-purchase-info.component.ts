import { Component, Inject, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DOCUMENT } from "@angular/common";
import { DialogBase } from 'mosaico-base';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-kanga-post-purchase-info',
  templateUrl: './kanga-post-purchase-info.component.html',
  styleUrls: []
})
export class KangaPostPurchaseInfoComponent extends DialogBase implements OnInit, OnDestroy {
  redirectUrl: string | null = null;
  subs = new SubSink();
  private readonly redirectAfterMilliseconds = 5000;
  countdown = this.redirectAfterMilliseconds / 1000;
  isRedirectionActive = true;
  @Input() externalSystemName: string;

  private countdownTimer: any;
  private redirectTimer: any;

  constructor(@Inject(DOCUMENT) private document: Document, private modal: NgbModal) {
    super(modal);
    this.extraOptions = {
      modalDialogClass: "mosaico-payment-modal"
    };
  }

  open(payload?: any): void {
    this.redirectUrl = payload;
    clearInterval(this.countdownTimer);
    clearInterval(this.redirectTimer);
    this.isRedirectionActive = true;
    if (this.redirectUrl && this.redirectUrl.length > 0) {
      this.countdown = this.redirectAfterMilliseconds / 1000;
      this.countdownTimer = setInterval(() => {
        if(this.countdown > 0) {
          this.countdown -= 1;
        }
      }, 1000);

      this.redirectTimer = setTimeout(() => {
        this.document.location.href = this.redirectUrl;
      }, this.redirectAfterMilliseconds);

      this.subs.sink = this.closed.subscribe((res) => {
        clearInterval(this.countdownTimer);
        clearInterval(this.redirectTimer);
      });
    }
    else {
      this.isRedirectionActive = false;
    }
    super.open(payload);
  }

  ngOnInit(): void {

  }

  ngOnDestroy() {
    clearInterval(this.countdownTimer);
    clearInterval(this.redirectTimer);
    this.subs.unsubscribe();
  }

  done(): void {
    this.modalRef.close(true);
  }
}
