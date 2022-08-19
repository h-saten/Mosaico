import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { BlockchainService, TranslationService } from 'mosaico-base';
import { ToastrService } from 'ngx-toastr';
import { selectProjectPreviewToken } from '../../../store';

@Component({
  selector: 'app-blockchain-address',
  templateUrl: './blockchain-address.component.html',
  styleUrls: ['./blockchain-address.component.scss']
})
export class BlockchainAddressComponent implements OnInit {
  link: string;
  isLoaded = false;

  constructor(private store: Store, private translateService: TranslateService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.store.select(selectProjectPreviewToken).subscribe((res) => {
      if(res && res.address){
        this.link = res.address;
        this.isLoaded = true;
      }
    });
  }

  onCopied(): void {
    this.translateService.get('PROJECT_OVERVIEW.SIDEBAR.ADDRESS_COPIED_MESSAGE').subscribe((t) => {
      this.toastr.success(t);
    });
  }

}
