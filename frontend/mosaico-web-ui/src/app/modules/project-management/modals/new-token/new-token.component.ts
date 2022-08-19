import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { DialogBase } from 'mosaico-base';
import { Blockchain } from 'mosaico-wallet';
import { take } from 'rxjs/operators';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { selectProjectCompany } from '../../store/project.selectors';

@Component({
  selector: 'app-new-token',
  templateUrl: './new-token.component.html',
  styleUrls: ['./new-token.component.scss']
})
export class NewTokenComponent extends DialogBase implements OnInit {
  @Input() projectId: string;
  @Input() companyId: string;
  isLoading = false;
  sub = new SubSink();
  networks: Blockchain[] = [];
  companyNetwork: string;

  constructor(modal: NgbModal, private store: Store) {
    super(modal);
    this.extraOptions = { scrollable: true };
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectCurrentActiveBlockchains).pipe(take(1)).subscribe((res) => {
      this.networks = res;
      this.sub.sink = this.store.select(selectProjectCompany).subscribe((res) => {
        if(res && res.network) {
          this.companyNetwork = res?.network;
        }
      });
    });
  }

  onCreated(id: string): void {
    this.modalRef.close(id);
  }
}
