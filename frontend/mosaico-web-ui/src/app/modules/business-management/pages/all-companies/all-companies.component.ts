import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { FormBase } from 'mosaico-base';
import { CompanyList, CompanyService } from 'mosaico-dao';
import { Blockchain } from 'mosaico-wallet';
import { SubSink } from 'subsink';
import { selectCurrentActiveBlockchains } from '../../../../store/selectors';

@Component({
  selector: 'app-all-companies',
  templateUrl: './all-companies.component.html',
  styleUrls: ['./all-companies.component.scss']
})
export class AllCompaniesComponent extends FormBase implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput: ElementRef<HTMLInputElement>;

  companies: CompanyList[] = [];
  skip = 0;
  take = 5;
  total = 0;
  loaded = this.take;
  loading = false;
  hideLoadMoreButton = false;
  sub = new SubSink();
  blockchainMap:{ [key: string]: Blockchain } = {};
  blockchains: Blockchain[] = [];

  showSearchFormMobile = false;

  constructor(
    private companyService: CompanyService,
    private store: Store,
    private formBuilder: FormBuilder) {
    super();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.createForm();
    this.getCompanies();
    this.sub.sink = this.store.select(selectCurrentActiveBlockchains).subscribe((b) => {
      this.blockchains = b;
      if(b && b.length > 0) {
        this.blockchainMap = {};
        b.forEach((b) => {
          this.blockchainMap[b.name] = b;
        });
      }
    });
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      search: [''],
    });
  }

  getCompanies(skip: number = this.skip, take: number = this.take, search: string = null) {
    search = this.form.get('search')?.value;
    this.loaded = this.take;
    this.companyService.getCompaniesListPublicly(skip, take, search).subscribe((res) => {
      if (res && res.data) {
        this.companies = res.data.entities;
        this.total = res.data.total;
        if (this.total <= this.take) {
          this.hideLoadMoreButton = true;

        }
      }
    }, (error: HttpErrorResponse) => {
    });
  }

  search(filter: any) {
    this.skip = 0;
    this.take = 5;
    this.total = 0;
    this.loaded = this.take;

    this.getCompanies(this.skip, this.take, filter.target.value);
  }

  loadMore() {
    this.loading = true;
    this.skip += this.take;
    this.loaded += this.take;
    let searchText = this.form.get('search')?.value;

    this.companyService.getCompaniesListPublicly(this.skip, this.take, searchText).subscribe((res) => {
      if (res && res.data) {
        this.companies.push(...res.data.entities);
        if (this.loaded >= this.total) {
          this.hideLoadMoreButton = true;
          this.skip = 0;
          this.take = 5;
          this.total = 0;
          this.loaded = this.take;
        }
        this.loading = false;
      }
    })
  }

  showSearchFormMobileFunc(): void {
    this.showSearchFormMobile = !this.showSearchFormMobile;
    if(this.showSearchFormMobile) {
      setTimeout(()=>{
        this.searchInput.nativeElement.focus();
      })
    }
  }
}
