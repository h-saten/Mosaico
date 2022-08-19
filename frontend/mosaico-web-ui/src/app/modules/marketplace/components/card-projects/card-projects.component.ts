import { Component, HostListener, Input, OnInit } from '@angular/core';
import { ProjectsList } from 'mosaico-project';

@Component({
  selector: 'app-card-projects',
  templateUrl: './card-projects.component.html',
  styleUrls: ['./card-projects.component.scss']
})
export class CardProjectsComponent implements OnInit {

  @Input() project: ProjectsList;

  tokenPrice = 0;
  tokenSymbol = '';

  softCapAmount = 0;
  hardCapAmount = 0;

  raisedCapital = 0;
  raisedCapitalPercentage = 0;
  numberOfBuyers = 0;

  constructor() { }

  ngOnInit(): void {

    if (this.project) {
      this.softCapAmount = this.project.softCap;
      this.hardCapAmount = this.project.isSoftCapAchieved ? this.project.hardCap : this.project.softCap;

      this.raisedCapital = this.project.raisedCapital;
      this.raisedCapitalPercentage = this.project.isSoftCapAchieved ? this.project.raisedCapitalPercentage : this.project.raisedCapitalSoftCapPercentage;
      this.numberOfBuyers = this.project.numberOfBuyers;

      this.tokenPrice = this.project.activeStage?.tokenPrice;
      this.tokenSymbol = this.project.tokenSymbol;
    }
  }
}
