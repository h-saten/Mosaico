import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, DefaultHeaders, SuccessResponse } from 'mosaico-base';
import { GetBlockchainsResponse } from '../responses/get-blockchains.response';
import { Observable } from 'rxjs';
import { DeploymentEstimate } from '../models';

@Injectable({
    providedIn: 'root'
})

export class ActiveBlockchainService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getActiveBlockchains(): Observable<SuccessResponse<GetBlockchainsResponse>> {
        return this.http.get<SuccessResponse<GetBlockchainsResponse>>(`${this.baseUrl}/core/api/chains`, { headers: DefaultHeaders });
    }

    getEstimates(): Observable<SuccessResponse<DeploymentEstimate[]>> {
        return this.http.get<SuccessResponse<DeploymentEstimate[]>>(`${this.baseUrl}/core/api/chains/estimates`, { headers: DefaultHeaders });
    }
}