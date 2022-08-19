import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { InvestorAccountBalanceResponse } from '../responses';
import { InvestorTransactionsResponse, UserKangaBalanceResponse } from '../responses';
import {CurrencyLogo} from "../../services";
import {map} from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})

export class KangaWalletService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getBalance(): Observable<SuccessResponse<InvestorAccountBalanceResponse>> {
        return this.http.get<SuccessResponse<InvestorAccountBalanceResponse>>(`${this.baseUrl}/core/api/KangaUser/InvestorWallet`, { headers: DefaultHeaders });
    }

    getTransactions(token?: string, skip: number = 0, take: number = 10): Observable<SuccessResponse<InvestorTransactionsResponse>> {
        return this.http.get<SuccessResponse<InvestorAccountBalanceResponse>>(`${this.baseUrl}/core/api/KangaUser/InvestorTransactions`, { headers: DefaultHeaders });
    }

    getUserKangaBalance(): Observable<SuccessResponse<UserKangaBalanceResponse>> {
      return this.http.get<SuccessResponse<UserKangaBalanceResponse>>(
        `${this.baseUrl}/core/api/kanga/account/balance`,
        { headers: DefaultHeaders });
    }
}
