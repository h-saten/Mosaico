import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService } from 'mosaico-base';
import { ExternalExchange, ExternalExchangeService, TokenExternalExchange, TokenService } from 'mosaico-wallet';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { PERMISSIONS } from '../../../constants';
import { selectProjectPreviewToken, selectPreviewProjectPermissions } from '../../../store';

@Component({
  selector: 'app-exchange-list',
  templateUrl: './exchange-list.component.html',
  styleUrls: ['./exchange-list.component.scss']
})
export class ExchangeListComponent implements OnInit {
  exchanges: ExternalExchange[] = [];
  tokenExchanges: TokenExternalExchange[] = [];
  canEdit = false;
  subs = new SubSink();
  tokenId: string;
  isLoading = false;

  constructor(private store: Store, private exchangeService: ExternalExchangeService, private tokenService: TokenService,
    private translateService: TranslateService, private toastr: ToastrService, private errorHandler: ErrorHandlingService) { }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
      this.getTokenInformation();
      if (this.canEdit) {
        this.getExchanges();
      }
    });
  }

  getExchanges(): void {
    this.subs.sink = this.exchangeService.getExternalExchanges().subscribe((res) => {
      if (res && res.data) {
        this.exchanges = res.data.filter((e) => e.isDisabled === false);
      }
    });
  }

  getTokenInformation(): void {
    this.subs.sink = this.store.select(selectProjectPreviewToken).subscribe((t) => {
      if (t) {
        this.tokenExchanges = t.exchanges;
        this.tokenId = t.id;
      }
    });
  }

  navigateToExchange(e: ExternalExchange): void {
    if (e && e.url && e.url.length > 0) {
      window.open(e.url, "_blank", 'noopener');
    }
  }

  toggleExchange(id: string): void {
    if (this.canEdit) {
      if (id && this.exchanges && this.exchanges.length > 0) {
        const exchange = this.exchanges.find((t) => t.id === id);
        const tokenExchange = this.tokenExchanges?.find((x) => x.externalExchange?.id === exchange.id);
        if (tokenExchange) {
          this.disableExchange(id);
        }
        else {
          this.enableExchange(id);
        }
      }
    }
  }

  isEnabled(id: string): boolean {
    return this.tokenExchanges && this.tokenExchanges?.findIndex((te) => te.externalExchange?.id === id) >= 0;
  }

  enableExchange(id: string): void {
    if (!this.isLoading) {
      this.isLoading = true;
      this.subs.sink = this.tokenService.upsertExternalExchange(this.tokenId, { externalExchangeId: id }).subscribe((res) => {
        if (res) {
          this.translateService.get('PROJECT_OVERVIEW.EXCHANGES.MESSAGES.ACTIVATED').subscribe((t) => {
            this.toastr.success(t);
          });
          setTimeout(() => { this.isLoading = false; }, 2000);
        }
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isLoading = false; });
    }
  }

  disableExchange(id: string): void {
    if (!this.isLoading) {
      this.isLoading = true;
      this.subs.sink = this.tokenService.deleteExternalExchange(this.tokenId, id).subscribe((res) => {
        if (res) {
          this.translateService.get('PROJECT_OVERVIEW.EXCHANGES.MESSAGES.DEACTIVATED').subscribe((t) => {
            this.toastr.success(t);
          });
          setTimeout(() => { this.isLoading = false; }, 2000);
        }
      }, (error) => { this.errorHandler.handleErrorWithToastr(error); this.isLoading = false; });
    }
  }

}
