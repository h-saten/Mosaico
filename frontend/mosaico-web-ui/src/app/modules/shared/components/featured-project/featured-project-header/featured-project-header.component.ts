import { Component, OnInit, Input, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService } from 'mosaico-base';

import { ProjectService, ProjectsList } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { selectIsAuthorized } from '../../../../user-management/store/user.selectors';

enum EProjectStatus {
  Closed = 'CLOSED',
  Upcoming = 'UPCOMING',
  PublicSale = "PUBLIC_SALE",
  PreSale = "PRE_SALE",
  PrivateSale = "PRIVATE_SALE"
}

@Component({
  selector: 'app-featured-project-header',
  templateUrl: './featured-project-header.component.html',
  styleUrls: ['./featured-project-header.component.scss']
})
export class FeaturedProjectHeaderComponent implements OnInit, OnDestroy, OnChanges {
  @Input() project: ProjectsList;
  @Input() isProjectItem: boolean = false;
  @Input() isFirst = false;
  startDate: Date;
  subs = new SubSink();
  isAuthorized = false;
  projectStatus = EProjectStatus;

  constructor(private service: ProjectService, private store: Store, private toastr: ToastrService, private translateService: TranslateService,
    private errorHandler: ErrorHandlingService) { }

  ngOnChanges(changes: SimpleChanges): void {
    // if(this.project?.activeStage?.startDate && this.project?.activeStage?.startDate?.length > 0) {
    //   this.startDate = new Date(this.project.activeStage.startDate)
    // }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.store.select(selectIsAuthorized).subscribe((res) => {
      this.isAuthorized = res;
    });
  }


  like(): void {
    if(!this.isAuthorized) {
      this.translateService.get('PROJECT.LIKE.UNAUTHORIZED').subscribe((t) => {
        this.toastr.warning(t);
      });
      return;
    }
    if (this.project.likedByUser === false) {
      this.subs.sink = this.service.like(this.project.id).subscribe((res) => {
        this.project.likeCount++;
        this.project.likedByUser = true;
      }, (error) => {
        this.errorHandler.handleErrorWithToastr(error);
      });
    }
  }

}
