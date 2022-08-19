import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { ProjectTransactions } from 'mosaico-project';
import { SubSink } from 'subsink';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { FilterMetadata, LazyLoadEvent } from 'primeng/api';
import { selectProjectPreview } from '../../../../store';
import { SalesAgent, SalesAgentService, Token, TransactionService } from 'mosaico-wallet';
import { FilterParams, SortingParams, FilterOptions, SortOrders, ErrorHandlingService } from 'mosaico-base';
import { BlockchainService } from '../../../../../../../../projects/mosaico-base/src/lib/services/blockchain.service';
import { selectCurrentActiveBlockchains } from 'src/app/store/selectors';
import { selectProjectPreviewToken } from '../../../../store/project.selectors';

@Component({
  selector: 'app-project-transactions',
  templateUrl: './project-transactions.component.html',
  styleUrls: ['./project-transactions.component.scss']
})
export class ProjectTransactionsComponent implements OnInit, OnDestroy {
  showFilters = false;
  isLoading: boolean = true;
  sub: SubSink = new SubSink();
  transaction: ProjectTransactions[] = [];
  filters: FilterParams[] = [];
  sorts: SortingParams[] = [];
  projectId: string;
  token: Token;
  expandedRow = -1;
  currentSkip: number = 0;
  currentTake: number = 10;
  page = 1;
  pageSize = 10;
  totalRecords: number;
  projectName: string;

  paymentMethods: string[] = [
    'METAMASK',
    'MOSAICO_WALLET',
    'BANK_TRANSFER',
    'CREDIT_CARD',
    'RAMP',
    'TRANSAK',
    'BINANCE'
  ];
  transactionStatuses: string[] = [
    'PENDING',
    'REFUNDED',
    'CONFIRMED',
    'CANCELED',
    'FAILED',
    'EXPIRED'
  ];
  filteredStatuses: string[];
  filteredPaymentMethod: string[];
  filteredTimeRange: Date[];
  filterCorrelationId: string;
  agents: SalesAgent[] = [];

  constructor(
    private store: Store,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private transactionService: TransactionService,
    private errorHandler: ErrorHandlingService,
    private blockchainService: BlockchainService,
    private salesAgentService: SalesAgentService
  ) { }

  ngOnInit(): void {
    this.getProjectId();
    this.getAgents();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  applyFilters(): void {
    this.currentSkip = 0;
    this.currentTake = 10;
    this.reloadTransactions();
  }

  clearFilters(): void {
    this.filteredPaymentMethod = [];
    this.filteredStatuses = [];
    this.filteredTimeRange = [];
    this.filterCorrelationId = null;
    this.reloadTransactions();
  }

  fetchTransactions(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;
    this.reloadTransactions();
  }

  private reloadTransactions(): void {
    const params = { skip: this.currentSkip, take: this.currentTake, projectId: this.projectId };
    this.sub.sink = this.transactionService.getProjectTransactions(params.projectId, params.skip, params.take,
      this.filteredTimeRange ? this.filteredTimeRange[0]?.toISOString() : null,
      this.filteredTimeRange ? this.filteredTimeRange[1]?.toISOString() : null,
      this.filteredStatuses ? this.filteredStatuses : null,
      this.filteredPaymentMethod ? this.filteredPaymentMethod : null, this.filterCorrelationId)
      .subscribe((res) => {
        this.transaction = res?.data?.entities;
        this.totalRecords = res?.data?.total;
        this.isLoading = false;
    }, (error) => { this.errorHandler.handleErrorWithToastr(error); });
  }

  getProjectId(): void {
    this.sub.sink = this.store.select(selectProjectPreview).subscribe((project) => {
      if (project?.project?.id) {
        this.projectId = project.project?.id;
        this.projectName = project.project?.title;
      }
    });
    this.sub.sink = this.store.select(selectProjectPreviewToken).subscribe((res) => {
      this.token = res;
    });
  }

  getValueByKeyForNumberEnum(valueKey: string): SortOrders {
    return <SortOrders>Object.entries(SortOrders).find(([key, val]) => key === valueKey)?.[1];
  }

  redirectToEtherscan(hash: string): void {
    if (this.token) {
      this.sub.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((chains) => {
        const chain = chains.find((c) => c.name === this.token.network);
        if (chain) {
          const link = this.blockchainService.getTransactionLink(hash, chain.etherscanUrl);
          window.open(link, "_blank", 'noopener');
        }
      });
    }
  }

  onExportCsv() {
    this.sub.sink = this.transactionService.exportTransactions(this.projectId, 'CSV', this.filteredTimeRange ? this.filteredTimeRange[0]?.toISOString() : null,
    this.filteredTimeRange ? this.filteredTimeRange[1]?.toISOString() : null,
    this.filteredStatuses ? this.filteredStatuses : null,
    this.filteredPaymentMethod ? this.filteredPaymentMethod : null).subscribe(
      res => {
        var url = URL.createObjectURL(res?.body);
        var downloadLink = document.createElement("a");
        downloadLink.href = url;
        const now = new Date();
        downloadLink.download = `${this.projectName}_${now.getFullYear()}-${now.getMonth()}-${now.getDay()}-${now.getHours()}${now.getMinutes()}.csv`;

        document.body.appendChild(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);
      },
      error => this.toastr.error(this.translateService.instant('PROJECT_SETTINGS.TRANSACTIONS.MESSAGES.NO_TRANSACTIONS_TO_BE_EXPORTED'))
    );
  }

  toggleRow(index: number): void {
    if(index !== this.expandedRow) {
      this.expandedRow = index;
    }
    else {
      this.expandedRow = -1;
    }
  }

  getAgents(): void {
    this.sub.sink = this.salesAgentService.getSalesAgents().subscribe((res) => {
      this.agents = res.data;
    });
  }
}
