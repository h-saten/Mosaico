import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService, DefaultHeaders, SuccessResponse } from 'mosaico-base';
import { Observable, of } from 'rxjs';
import { CreateVestingCommand } from '../commands';
import { PersonalVesting } from '../models/personal-vesting';
import { PrivateSaleVesting } from '../models/private-sale-vesting';
import { PersonalVestingResponse } from '../responses';

@Injectable({
    providedIn: 'root'
})
export class VestingService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public getPrivateSaleVesting(tokenId: string): Observable<SuccessResponse<PrivateSaleVesting[]>> {
        return of({data: [], ok: true});
    }

    public createVesting(command: CreateVestingCommand): Observable<SuccessResponse<string>> {
        return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/vesting`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public getPersonalVesting(tokenId: string): Observable<SuccessResponse<PersonalVestingResponse>> {
        return this.http.get<SuccessResponse<PersonalVestingResponse>>(`${this.baseUrl}/core/api/vesting?tokenId=${tokenId}&type=PERSONAL`, { headers: DefaultHeaders });
    }

    public redeploy(id: string): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/vesting/${id}/contract`, null, { headers: DefaultHeaders });
    }
}