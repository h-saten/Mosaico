import {Component, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {Token, TokenService} from 'mosaico-wallet';
import {SubSink} from 'subsink';
import {selectCompanyPreview} from '../../store';
import {CompanyService} from "mosaico-dao";

@Component({
  selector: 'app-company-holders',
  templateUrl: './company-holders.component.html',
  styleUrls: ['./company-holders.component.scss']
})
export class CompanyHoldersComponent implements OnInit {

  private subs: SubSink = new SubSink();

  companyId: string;
  isLoaded = false;
  isInternallyLoading = false;
  isLoading = false;

  tokens: Token[];

  constructor(
    private store: Store,
    private companyService: CompanyService,
    private tokenService: TokenService
  ) { }

  async ngOnInit(): Promise<void> {
    await this.getCompany();
  }

  private async getCompany(): Promise<void> {
    this.subs.sink = this.store.select(selectCompanyPreview).subscribe((res) => {
      this.companyId = res.id;
      this.getCompanyTokens();
    });
  }

  private getCompanyTokens(force = false): void {
    if(this.companyId && (!this.isLoaded || force)){
      this.tokenService.getCompanyTokens(this.companyId).subscribe((res) => {
        this.tokens = res.data;
        this.isLoaded = true;
      });
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
