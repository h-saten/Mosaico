import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProjectService, SubscribePrivateSaleResponse } from 'mosaico-project';
import { BehaviorSubject } from 'rxjs';
import { Router, Route, ActivatedRoute } from '@angular/router';
import { SubSink } from 'subsink';
import { ErrorHandlingService } from 'mosaico-base';

@Component({
  selector: 'app-authorize-private-sale',
  templateUrl: './authorize-private-sale.component.html',
  styleUrls: ['./authorize-private-sale.component.scss']
})
export class AuthorizePrivateSaleComponent implements OnInit, OnDestroy {
  isLoading$ = new BehaviorSubject<boolean>(false);
  response: SubscribePrivateSaleResponse;
  subs = new SubSink();

  constructor(private projectService: ProjectService, private router: Router, private route: ActivatedRoute,
    private errorHandler: ErrorHandlingService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.isLoading$.next(true);
    const code = this.route.snapshot.queryParamMap.get('authCode');
    if (!code || code.length === 0) {
      this.router.navigateByUrl('/projects');
    }
    else {
      this.subs.sink = this.projectService.subscribePrivateSale(code).subscribe((res) => {
        this.response = res.data;
        this.isLoading$.next(false);
      }, (error) => {
        this.errorHandler.handleErrorWithRedirect(error, '/projects');
      });
    }
  }

}
