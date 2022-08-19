import {Component, OnDestroy, OnInit,ViewChild,ElementRef } from '@angular/core';
import {Store} from '@ngrx/store';
import {ToastrService} from 'ngx-toastr';
import {ErrorHandlingService} from 'mosaico-base';
import {
  selectCompanyPermissions,
  selectCompanyPreview,
  selectUserSubscribedCompany,
  setCurrentCompany
} from 'src/app/modules/business-management/store';
import {SubSink} from 'subsink';
import {COMPANY_PERMISSIONS} from '../../constants';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {EditCompanyLogoComponent} from '../../modals';
import {AuthService, DEFAULT_MODAL_OPTIONS, FileParameters,BlockchainNetworkType} from 'mosaico-base';
import {CompanyWalletService, Token, TokenService, TransactionService,Blockchain,CompanyWalletBalance} from 'mosaico-wallet';
import {Company, CompanyService,CompanyDocument,UploadCompanyDocumentCommand } from 'mosaico-dao';
import {SubscriptionToNewsletterComponent} from '../../modals/subscription-to-newsletter/subscription-to-newsletter.component';
import {Observable} from 'rxjs';
import { setCurrentBlockchain } from 'src/app/store/actions';
import {selectIsAuthorized} from 'src/app/modules/user-management/store';
import { selectBlockchain, selectCurrentActiveBlockchains } from '../../../../store/selectors';
import { languages } from 'src/app/modules/shared/models';
// import Swiper core and required components
import SwiperCore , {
  Lazy,
  Navigation,
  Pagination,
  Scrollbar,
  Mousewheel,
  A11y,
  Virtual,
  Zoom,
  Autoplay,
  Thumbs,
  Controller,
  EffectCoverflow
} from 'swiper';
import { Router } from '@angular/router';

// install Swiper components
SwiperCore.use([
  Navigation,
  Pagination,
  Lazy,
  Mousewheel,
  Scrollbar,
  A11y,
  Virtual,
  Zoom,
  Autoplay,
  Thumbs,
  Controller,
  EffectCoverflow
]);

@Component({
  selector: 'app-company-overview',
  templateUrl: './company-overview.component.html',
  styleUrls: ['./company-overview.component.scss']
})
export class CompanyOverviewComponent implements OnInit, OnDestroy {
  subs: SubSink = new SubSink();
  canEdit: boolean = false;
  private sub: SubSink = new SubSink();
  company: Company;
  tokens: Token[] = [];
  isLoaded = false;
  isLoading = false;
  isUserSubscribeDao= false;
  isAuthorized$: Observable<boolean>;
  documents: CompanyDocument[] = [];
  languages = languages;
  selectedLanguage = "";
  selectedNetwork: BlockchainNetworkType;
  walletAddress: string;
  networks: Blockchain[] = [];

  constructor(
    private companyService: CompanyService,
    private companyWalletService: CompanyWalletService,
    private tokenService: TokenService,
    private modalService: NgbModal,
    public auth: AuthService,
    private router: Router,
    private toastr: ToastrService, private errorHandler: ErrorHandlingService,
    private store: Store) { }

  async ngOnInit(): Promise<void> {
    await this.getCompanyPermissions();
    await this.getCompany();
    this.isAuthorized$ = this.store.select(selectIsAuthorized);
    this.sub.sink = this.store.select(selectUserSubscribedCompany).subscribe((res) => {
      this.isUserSubscribeDao = res;
    });
    this.subs.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((res) => {
      this.networks = res;
    });

    this.getCompanyDocuments();
  }

  getCompanyWalletSummary(network: BlockchainNetworkType): void {
    if (this.company.id && !this.isLoading) {
      this.isLoading = true;
      this.subs.sink = this.companyWalletService.getCompanyWalletTokens(this.company.id).subscribe((res) => {
        if (res && res.data) {
          this.saveCompanyWalletInStore(res.data, network);
          this.saveCurrentBlockchain(network);
        }
        this.isLoading = false;
      }, (error) => { this.isLoading = false; });
    }
  }

  saveCompanyWalletInStore(balance: CompanyWalletBalance, network: BlockchainNetworkType): void {
    this.selectedNetwork = network;
  }

  saveCurrentBlockchain(network: BlockchainNetworkType): void {
    if (network) {
      this.store.dispatch(setCurrentBlockchain({ blockchain: network }));
    }
  }

  async getCompany(): Promise<void> {
    this.sub.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
        this.company = res;
        this.selectedNetwork = this.company.network;
        this.getCompanyTokens();
    });
  }

  getCompanyTokens(force = false): void {
    if(this.company && (!this.isLoaded || force)){
      this.tokenService.getCompanyTokens(this.company.id).subscribe((res) => {
        if (res) {
          this.tokens = res.data;
          this.isLoaded = true;
        }
      });
    }
  }

  openLogoEditing(): void {
    if(this.canEdit && this.company) {
      const modalRef = this.modalService.open(EditCompanyLogoComponent, DEFAULT_MODAL_OPTIONS);
      modalRef.componentInstance.currentImg = this.company.logoUrl;
      modalRef.componentInstance.companyId = this.company.id;
      this.sub.sink = modalRef.closed.subscribe((res?: FileParameters) => {
        if(res){
          setTimeout(() => {
            this.reloadCompany();
          }, 2000);
        }
      });
    }
  }

  reloadCompany(): void {
    if(this.company){
      this.sub.sink = this.companyService.getCompany(this.company.id).subscribe((res) => {
        if(res && res.data) {
          this.store.dispatch(setCurrentCompany(res.data?.company));
        }
      });
    }
  }

  private async getCompanyPermissions(): Promise<void> {
    this.sub.sink = this.store.select(selectCompanyPermissions).subscribe((res) => {
      this.canEdit = res && (res[COMPANY_PERMISSIONS.CAN_EDIT_DETAILS]);
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  openModalSubscriptionToNewsletter(): void {
    this.sub.sink = this.isAuthorized$.pipe().subscribe((res) => {
      if(res === true && this.company.id){
        const modalRef = this.modalService.open(SubscriptionToNewsletterComponent, DEFAULT_MODAL_OPTIONS);
        modalRef.componentInstance.isUserSubscribeDao = this.isUserSubscribeDao;
        modalRef.componentInstance.daoId = this.company.id;

      }else{
        this.auth.loginWithRedirect(this.router.url);
      }
    });
  }

  getCompanyDocuments(force = false): void{
    if ((!this.isLoaded || force === true) && !this.isLoading) {
      this.isLoading = true;
      this.companyService.getCompanyDocuments(this.company.id).subscribe((res) => {
        if (res && res.data) {
          this.documents = res.data.documents;
          if (this.documents && !this.canEdit) {
            this.documents = this.documents.filter((d) => d.id && d.id.length > 0);
          }
        }
        this.isLoading = false;
        this.isLoaded = true;
      }, (error) => { this.isLoaded = true; this.isLoading = false; this.errorHandler.handleErrorWithToastr(error); });
    }
  }

  changeLan(val):void{
    this.selectedLanguage = val.lang;
  }

  saveDocument(event: any): void {
    if (event && event.target && event.target.files && event.target.files.length > 0) {
      //var selectedType = this.documents.find(x=>x.id == this.company.id);

      const command: UploadCompanyDocumentCommand = {
        content: event.target.files,
        language: this.selectedLanguage == "" ? "en" : this.selectedLanguage,
      }
      this.subs.sink = this.store.select(selectCompanyPreview).subscribe((company) => {
        if (company) {
          this.subs.sink = this.companyService.uploadCompanyDocument(company.id, command).subscribe((result) => {
            if (result && result.data) {
              this.toastr.success('Document was successfully created');
              this.getCompanyDocuments(true);
            }
          }, (error) => {
            this.errorHandler.handleErrorWithToastr(error);
          });
        }
      });
    }
  }

  openFile(d: CompanyDocument): void {
    if(d && d.url && d.url.length > 0){
      window.open(d.url, '_blank');
    }
  }

}
