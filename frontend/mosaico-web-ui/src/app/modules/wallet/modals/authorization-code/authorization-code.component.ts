import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { DialogBase, ErrorHandlingService } from 'mosaico-base';
import { ProjectService } from 'mosaico-project';
import { SubSink } from 'subsink';
import { BehaviorSubject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-authorization-code',
  templateUrl: './authorization-code.component.html',
  styleUrls: ['./authorization-code.component.scss']
})
export class AuthorizationCodeComponent extends DialogBase implements OnInit, OnDestroy {
  @Input() projectId: string;
  stageId: string;
  isLoading$ = new BehaviorSubject<boolean>(false);
  sub = new SubSink();
  code: string;

  constructor(modal: NgbModal, private projectService: ProjectService, private errorHandler: ErrorHandlingService,
    private toastr: ToastrService) {
    super(modal);
    this.extraOptions = { scrollable: true, modalDialogClass: "mosaico-payment-modal" };
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {

  }

  onCopied(): void {
    this.toastr.success('Link copied');
  }

  open(stageId: string): void {
    this.stageId = stageId;
    this.isLoading$.next(true);
    this.sub.sink = this.projectService.getCode(this.projectId, this.stageId).subscribe((res) => {
      this.code = `${window.location.origin}/project/privateSale?authCode=${res.data}`;
      this.isLoading$.next(false);
    }, (error) => {
      this.modalRef.close(false);
      this.errorHandler.handleErrorWithToastr(error);
    });
    super.open();
    this.modalRef.closed.subscribe((r) => {
      this.code = null;
    });
  }

}
