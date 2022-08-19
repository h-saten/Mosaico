import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { SalesAgent } from '../models/sales-agent';

@Injectable({
    providedIn: 'root'
})
export class SalesAgentService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getSalesAgents(company?: string): Observable<SuccessResponse<SalesAgent[]>> {
        let params: {};
        if(company && company.length > 0) params = {...params, company};
        return this.http.get<SuccessResponse<SalesAgent[]>>(`${this.baseUrl}/core/api/agents`, { headers: DefaultHeaders, params });
    }
}