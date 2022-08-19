import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Project } from 'mosaico-project';
import { SubSink } from 'subsink';
import { selectPreviewProject, selectProjectPreviewToken } from '../../../store';

@Component({
  selector: 'app-project-sales-progress-bar',
  templateUrl: './project-sales-progress-bar.component.html',
  styleUrls: ['./project-sales-progress-bar.component.scss']
})
export class ProjectSalesProgressBarComponent implements OnInit {
  subs: SubSink = new SubSink();
  raisedCapitalPercentageForDisplay = 0;
  userCurrency?: string = 'USD';

  tokenPrice = 0;
  tokenSymbol = '';
  softCapAmount = 0;
  hardCapAmount = 0;
  raisedCapital = 0;
  raisedCapitalPercentage = 0;
  numberOfBuyers = 0;
  currentProject: Project;
  projectId = '';
  statusProject: string | undefined = '';
  isSoftCapAchieved: boolean = false;
  startDate: Date;
  endDate: Date;
  raisedCapitalInUSD: number;
  colorOfProgressBar = '';
  softCapTokens: number;
  hardCapTokens: number;
  softCapRaisedCapitalPercentage: number;

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProject).subscribe((prj) => {
      if (prj) {
        this.currentProject = prj;
        this.projectId = this.currentProject.id;
        if (this.currentProject.activeStage) {
          this.statusProject = this.currentProject.activeStage.status;
          this.tokenPrice = this.currentProject.activeStage.tokenPrice;
          this.startDate = new Date(this.currentProject.activeStage.startDate);
          this.endDate = new Date(this.currentProject.activeStage.endDate);
        } else {
          this.statusProject = this.currentProject.status;
        }
        this.softCapAmount = this.currentProject.softCapInUserCurrency;
        this.hardCapAmount = this.currentProject.hardCapInUserCurrency;
        this.raisedCapital = this.currentProject.raisedCapital;
        this.raisedCapitalPercentage = this.currentProject.isSoftCapAchieved ? this.currentProject.raisedCapitalPercentage : this.currentProject.raisedCapitalSoftCapPercentage;
        this.numberOfBuyers = this.currentProject.numberOfBuyers;
        this.softCapRaisedCapitalPercentage = this.currentProject.raisedCapitalSoftCapPercentage;
        this.raisedCapitalInUSD = this.currentProject.raisedCapitalInUSD;
        this.softCapTokens = this.currentProject.softCap;
        this.hardCapTokens = this.currentProject.hardCap;
        this.isSoftCapAchieved = this.currentProject.isSoftCapAchieved;
      }
    });
    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((t) => {
      if (t) {
        this.tokenSymbol = t?.symbol;
      }
    });
  }
}
