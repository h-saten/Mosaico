import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';

import { ProjectsList } from 'mosaico-project';
import { SubscriptionToNewsletterComponent } from '../../../../shared/modals/subscription-to-newsletter/subscription-to-newsletter.component';

@Component({
  selector: 'app-featured-project-footer',
  templateUrl: './featured-project-footer.component.html',
  styleUrls: ['./featured-project-footer.component.scss']
})
export class FeaturedProjectFooterComponent implements OnInit {
  @Input() project: ProjectsList;
  @Input() isProjectItem: boolean = false;
  @Input() isLandingPage: boolean = false;

  raisedCapitalPercentage: number;
  softCapRaisedCapitalPercentage: number;
  isSoftCapAchieved: boolean = false;

  btnText: string = 'Invest now';

  constructor(private router: Router, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.softCapRaisedCapitalPercentage = this.project.raisedCapitalSoftCapPercentage;
    this.raisedCapitalPercentage = this.project.isSoftCapAchieved ? this.project.raisedCapitalPercentage : this.project.raisedCapitalSoftCapPercentage;
  }

  subscribe(): void {
    if (this.project) {
      const modalRef = this.modalService.open(SubscriptionToNewsletterComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.isUserSubscribeProject = this.project.isUserSubscribeProject;
      modalRef.componentInstance.projectId = this.project.id;
    }
  }
}
