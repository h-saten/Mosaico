import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService, DefaultHeaders } from 'mosaico-base';
import { UpdateBankDetailsCommand } from '../commands';
import { BankDetailsResponse } from '../responses';

@Injectable({
    providedIn: 'root'
})
export class BankDetailsService {
    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public getBankDetails(id: string, version: string = '1.0'): Observable<BankDetailsResponse> {
        const requestData = { 'api-version': version};
        return this.http.get<BankDetailsResponse>(`${this.baseUrl}/core/api/projects/${id}/bank`, { headers: DefaultHeaders, params: requestData });
    }

    public updateBankDetails(id: string, command: UpdateBankDetailsCommand, version: string = '1.0',): Observable<BankDetailsResponse> {
        const requestData = { 'api-version': version};
        return this.http.post<BankDetailsResponse>(`${this.baseUrl}/core/api/projects/${id}/bank`, command, { headers: DefaultHeaders, params: requestData });
    }
}