import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyService } from 'mosaico-dao';

@Component({
  selector: 'app-company-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.scss']
})
export class InvitationComponent implements OnInit, OnDestroy {

  code = "";

  private sub: SubSink = new SubSink();

  constructor(
    private companyService: CompanyService,
    private store: Store,
    private toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router

  ) {
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    if (this.activatedRoute) {
      this.sub.sink = this.activatedRoute.params.subscribe(data => {
        const invitationId: string | null = data.id;
        if (invitationId && invitationId.length > 0) {
          this.code = invitationId;
        }
        else {
          this.router.navigateByUrl('/dao');
        }
      });
    }
  }

  acceptInvitation(): void {
    this.companyService.acceptInvitation(this.code).subscribe((res: any) => {
      if (res.data) {
        this.toastr.success('Invitation was accepted successfully');
        this.router.navigateByUrl('/dao/' + res.data);
      }
      else {
        this.toastr.error(res.message);
      }
    });
  }
}
