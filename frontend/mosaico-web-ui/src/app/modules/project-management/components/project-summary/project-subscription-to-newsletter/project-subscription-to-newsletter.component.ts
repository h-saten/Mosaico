import { Component, Input, OnInit } from '@angular/core';
import { SubscriptionToNewsletterComponent } from '../../../../shared/modals/subscription-to-newsletter/subscription-to-newsletter.component';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-project-subscription-to-newsletter',
  templateUrl: './project-subscription-to-newsletter.component.html',
  styleUrls: ['./project-subscription-to-newsletter.component.scss']
})
export class ProjectSubscriptionToNewsletterComponent implements OnInit {

  @Input() isUserSubscribeProject: boolean;
  @Input() projectId: string;

  constructor(
    private modalService: NgbModal,
  ) { }

  ngOnInit(): void {
  }

  openModalSubscriptionToNewsletter(): void {
    if (this.projectId) {
      const modalRef = this.modalService.open(SubscriptionToNewsletterComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.isUserSubscribeProject = this.isUserSubscribeProject;
      modalRef.componentInstance.projectId = this.projectId;
    }
  }
}
