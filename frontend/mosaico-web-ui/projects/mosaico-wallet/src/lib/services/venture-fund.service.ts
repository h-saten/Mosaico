import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { HttpClient } from "@angular/common/http";
import { VentureFundHistoryResponse } from '../responses/venture-fund-history.response';

@Injectable({
    providedIn: 'root'
})

export class VentureFundService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getFundHistory(name: string, from?: string, to?: string): Observable<SuccessResponse<VentureFundHistoryResponse>> {
        let params: any = { name };
        if(from && from.length > 0) {
            params = {...params, from};
        }
        if(to && to.length > 0) {
            params = {...params, to};
        }

        return this.http.get<SuccessResponse<VentureFundHistoryResponse>>(`${this.baseUrl}/core/api/fund`, { headers: DefaultHeaders, params });
    }
}