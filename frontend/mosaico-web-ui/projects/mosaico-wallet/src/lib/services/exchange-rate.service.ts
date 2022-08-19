import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { ExchangeRateResponse } from '../responses';
import { TokenPriceHistory } from '../models';

@Injectable({
    providedIn: 'root'
})
export class ExchangeRateService{
    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getRates(): Observable<SuccessResponse<ExchangeRateResponse>> {
        return this.http.get<SuccessResponse<ExchangeRateResponse>>(`${this.baseUrl}/core/api/rates`, { headers: DefaultHeaders });
    }

    getHistoricalRate(symbol: string, from?: string, to?: string): Observable<SuccessResponse<TokenPriceHistory>> {
        let params: any = { symbol };
        if(from && from.length > 0) {
            params = {...params, from};
        }
        if(to && to.length > 0) {
            params = {...params, to};
        }

        return this.http.get<SuccessResponse<TokenPriceHistory>>(`${this.baseUrl}/core/api/rates/history`, { headers: DefaultHeaders, params });
    }
}