import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ErrorHandlingService } from 'mosaico-base';
import { ProjectService, ProjectSubscriber } from 'mosaico-project';
import { LazyLoadEvent } from 'primeng/api';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-newsletter-subscribers',
  templateUrl: './newsletter-subscribers.component.html',
  styleUrls: ['./newsletter-subscribers.component.scss']
})
export class NewsletterSubscribersComponent implements OnInit, OnDestroy {
  @Input() projectId: string;
  subs = new SubSink();
  isLoading: boolean = true;
  subscribers: ProjectSubscriber[] = [];
  currentSkip: number = 0;
  currentTake: number = 10;
  page = 1;
  pageSize = 10;
  totalRecords: number;

  constructor(private projectService: ProjectService, private errorHandler: ErrorHandlingService, private store: Store) { }


  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
  }

  fetchSubscribers(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;
    this.reloadSubscribers();
  }

  reloadSubscribers(): void {
    this.subs.sink = this.projectService.getSubscribers(this.projectId, this.currentSkip, this.currentTake)
      .subscribe((res) => {
        this.subscribers = res?.data?.entities;
        this.totalRecords = res?.data?.total;
        this.isLoading = false;
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
  }

  onExportCsv() {
    this.subs.sink = this.projectService.exportSubscribers(this.projectId).subscribe(
      res => {
        var url = URL.createObjectURL(res?.body);
        var downloadLink = document.createElement("a");
        downloadLink.href = url;
        const now = new Date();
        downloadLink.download = `newsletter_subscribers_${now.getFullYear()}-${now.getMonth()}-${now.getDay()}-${now.getHours()}${now.getMinutes()}.csv`;

        document.body.appendChild(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);
      },
      error => {
        this.errorHandler.handleErrorWithToastr(error);
      });
  }

}
