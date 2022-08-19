import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, DefaultHeaders, SuccessResponse } from 'mosaico-base';
import { Observable } from 'rxjs';
import { TokenDistribution } from '../models/token-distribution';
import { UpsertTokenDistributionCommand } from '../commands/upsert-token-distribution.command';

@Injectable({
    providedIn: 'root'
})
export class TokenDistributionService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getTokenDistribution(id: string): Observable<SuccessResponse<TokenDistribution[]>> {
        return this.http.get<SuccessResponse<TokenDistribution[]>>(`${this.baseUrl}/core/api/tokens/${id}/distribution`, { headers: DefaultHeaders });
    }

    upsert(id: string, command: UpsertTokenDistributionCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/distribution`, JSON.stringify(command), { headers: DefaultHeaders });
    }
}