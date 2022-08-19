import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { ExternalExchange } from '../models/external-exchange';

@Injectable({
    providedIn: 'root'
})

export class ExternalExchangeService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getExternalExchanges(): Observable<SuccessResponse<ExternalExchange[]>> {
        return this.http.get<SuccessResponse<ExternalExchange[]>>(`${this.baseUrl}/core/api/exchanges`, { headers: DefaultHeaders });
    }
}