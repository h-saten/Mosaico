import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { DialogBase } from 'mosaico-base';
import { Blockchain } from 'mosaico-wallet';
import { take } from 'rxjs/operators';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { SubSink } from 'subsink';
import { selectCompanyPreview } from '../../store';

@Component({
  selector: 'app-company-import-token',
  templateUrl: './import-token.component.html',
  styleUrls: []
})
export class ImportCompanyTokenComponent extends DialogBase implements OnInit {
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
