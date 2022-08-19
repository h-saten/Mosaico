import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-section-follow-steps',
  templateUrl: './section-follow-steps.component.html',
  styleUrls: ['./section-follow-steps.component.scss']
})
export class SectionFollowStepsComponent implements OnInit {
  activeTab: number = 0;

  constructor() { }

  setActiveTab(activeTab: number) {
    this.activeTab = activeTab;
  }

  ngOnInit(): void {
  }

}
