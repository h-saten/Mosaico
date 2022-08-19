import { Component,ViewChild,ElementRef, Input, OnInit, SimpleChanges } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  //selector: 'app-page-intro-video',
  templateUrl: './page-intro-video.component.html',
  styleUrls: ['./page-intro-video.component.scss']
})
export class PageIntroVideoComponent implements OnInit {
  @Input() videoExternalLink: string;
  @Input() videoUrl: string;
  @Input() showVideoUrl: boolean;
  @ViewChild('videoPlayer') videoplayer: ElementRef;

  playVideoUrl:string;
  playVideoExternalLink;
  constructor(
    public activeModal: NgbActiveModal,
    private _sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    if(this.videoExternalLink)
    {
      this.videoExternalLink = this.videoExternalLink.replace("watch?v=","embed/") + "?autoplay=1";
    }
    this.playVideoUrl = this.videoUrl;
    this.playVideoExternalLink = this._sanitizer.bypassSecurityTrustResourceUrl(this.videoExternalLink);
    if(this.showVideoUrl)
    {
      this.videoplayer.nativeElement.play();
    }
  }
}
