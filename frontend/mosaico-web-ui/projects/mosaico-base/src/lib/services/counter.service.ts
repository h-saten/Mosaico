import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { DefaultHeaders, SuccessResponse } from '../utils';
import { Observable } from 'rxjs';
import { GetCountersResponse, GetKPIResponse } from '../responses';

@Injectable({
    providedIn: 'root'
})
export class CounterService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    get(): Observable<SuccessResponse<GetCountersResponse>> {
        return this.http.get<SuccessResponse<GetCountersResponse>>(`${this.baseUrl}/core/api/counters`, { headers: DefaultHeaders });
    }

    getKPIs(): Observable<SuccessResponse<GetKPIResponse>> {
        return this.http.get<SuccessResponse<GetKPIResponse>>(`${this.baseUrl}/core/api/kpis`, { headers: DefaultHeaders });
    }

}