import { Component, Input, OnInit } from '@angular/core';
import { SubscriptionToNewsletterComponent } from '../../../../shared/modals/subscription-to-newsletter/subscription-to-newsletter.component';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-status-follow-share',
  templateUrl: './status-follow-share.component.html',
  styleUrls: ['./status-follow-share.component.scss']
})
export class StatusFollowShareComponent implements OnInit {

  @Input() statusProject: string;
  @Input() canEdit: boolean;
  @Input() isUserSubscribeProject: boolean;
  @Input() projectId: string;

  currentLink: string;

  constructor(
    private toastr: ToastrService,
    private modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.currentLink = window.location.href;
  }

  onCopied(): void {
    this.toastr.success('Link copied');
  }


  openModalSubscriptionToNewsletter(): void {
    if (this.projectId) {
      const modalRef = this.modalService.open(SubscriptionToNewsletterComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.isUserSubscribeProject = this.isUserSubscribeProject;
      modalRef.componentInstance.projectId = this.projectId;
    }
  }
}
