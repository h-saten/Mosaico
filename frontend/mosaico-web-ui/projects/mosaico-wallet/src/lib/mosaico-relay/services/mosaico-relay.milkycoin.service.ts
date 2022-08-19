import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { RelayHeaders } from '../constants';

@Injectable({
    providedIn: 'root'
})
export class MosaicoRelayMilkyService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().relayUrl;
    }

    getMilkyTokenAbi(): Observable<SuccessResponse<any>> {
        return this.http.get<SuccessResponse<any>>(`${this.baseUrl}/api/milkycoin/tokens/abi`, { headers: RelayHeaders });
    }

    getMilkyDividendAbi(): Observable<SuccessResponse<any>> {
        return this.http.get<SuccessResponse<any>>(`${this.baseUrl}/api/milkycoin/staking/dividends/abi`, { headers: RelayHeaders });
    }

    getDividendStakingAbi(version: string = 'v1'): Observable<SuccessResponse<any>> {
        return this.http.get<SuccessResponse<any>>(`${this.baseUrl}/api/${version}/staking/dividend/abi`, { headers: RelayHeaders });
    }
}