import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, BlockchainNetworkType, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { GetPaymentCurrenciesResponse } from '../responses/get-payment-currencies.response';
@Injectable({
    providedIn: 'root'
})

export class PaymentCurrencyService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getPaymentCurrencies(network: BlockchainNetworkType = "Ethereum"): Observable<SuccessResponse<GetPaymentCurrenciesResponse>> {
        return this.http.get<SuccessResponse<GetPaymentCurrenciesResponse>>(`${this.baseUrl}/core/api/currencies?network=${network}`, { headers: DefaultHeaders });
    }
}