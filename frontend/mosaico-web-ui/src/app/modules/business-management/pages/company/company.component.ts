import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Store } from "@ngrx/store";
import { clearCompany, setCurrentCompany, setUserSubscribedCompany } from "../../store";
import { SubSink } from 'subsink';
import { BlockchainNetworkType, ErrorHandlingService } from 'mosaico-base';
import { Company, CompanyService } from 'mosaico-dao';
import { selectBlockchain } from '../../../../store/selectors';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit, OnDestroy {

  company: Company;
  sub: SubSink = new SubSink();
  renderView: Observable<boolean>;
  defaultPath = 'dao';
  mainAddress = '';
  network: BlockchainNetworkType = 'Ethereum';

  constructor(
    private activatedRoute: ActivatedRoute,
    private companyService: CompanyService,
    private store: Store,
    private router: Router,
    private errorHandler: ErrorHandlingService
  ) { }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectBlockchain).subscribe((b) => {
      this.network = b;
      if(!this.network || this.network.length === 0) {
        this.network = 'Ethereum';
      }
      this.getCompany();
    });
  }

  getCompany(): void {
    this.sub.sink = this.activatedRoute.paramMap.subscribe(data => {
      const companyId: string | null = data.get('id');
      if (companyId !== null) {
        this.companyService.getCompany(companyId).subscribe((res) => {
          if (res && res.data) {
            this.store.dispatch(setUserSubscribedCompany({isSubscribed: res.data.isSubscribed}));
            this.company = res.data.company;
            this.saveCompanyInStore(this.company);
            this.renderView = of(true);
          }
        }, (error: HttpErrorResponse) => {
          this.errorHandler.handleErrorWithRedirect(error, `'/${this.defaultPath}'`);
        });
      }
      else {
        this.router.navigateByUrl(`'/${this.defaultPath}'`);
      }
    });
  }

  saveCompanyInStore(company: Company): void {
    this.store.dispatch(setCurrentCompany(company));
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearCompany());
    this.sub.unsubscribe();
  }
}
