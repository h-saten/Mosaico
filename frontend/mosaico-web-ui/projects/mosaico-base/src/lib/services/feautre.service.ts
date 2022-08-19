import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Feature } from '../models';
import { DefaultHeaders, PaginationResponse, SuccessResponse } from '../utils';
import { ConfigService } from './config.service';

@Injectable({
    providedIn: 'root'
})
export class FeatureService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getSystemSettings(): Observable<SuccessResponse<PaginationResponse<Feature>>> {
      return this.http.get<SuccessResponse<PaginationResponse<Feature>>>(`${this.baseUrl}/core/api/features`, { headers: DefaultHeaders });
    }

}
