import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { DialogBase } from 'mosaico-base';
import { Blockchain } from 'mosaico-wallet';
import { take } from 'rxjs/operators';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { selectCompanyPreview } from '../../store/business.selectors';

@Component({
  selector: 'app-company-new-token',
  templateUrl: './new-token.component.html',
  styleUrls: ['./new-token.component.scss']
})
export class NewCompanyTokenComponent extends DialogBase implements OnInit {
  @Input() companyId: string;
  isLoading = false;
  @Output() created = new EventEmitter<string>();
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
    });
    this.sub.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
      this.companyNetwork = res?.network;
    });
  }

  onCreated(id: string): void {
    this.modalRef.close(id);
    if(id && id.length > 0) {
      this.created.emit(id);
    }
  }
}
