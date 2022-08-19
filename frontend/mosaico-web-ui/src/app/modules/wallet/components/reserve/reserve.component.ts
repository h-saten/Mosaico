import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import { TOKEN_PERMISSIONS } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectTokenPermissions } from '../../store';
import { TokenDashboardService } from '../../services/token-dashboard.service';

@Component({
  selector: 'app-reserve',
  templateUrl: './reserve.component.html',
  styleUrls: ['./reserve.component.scss']
})
export class ReserveComponent implements OnInit, OnDestroy {
  canEdit = false;
  subs = new SubSink();
  @Input() public service: TokenDashboardService;

  constructor(private store: Store) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectTokenPermissions).subscribe((res) => {
      this.canEdit = res && res[TOKEN_PERMISSIONS.CAN_EDIT] === true;
    });
  }

}
