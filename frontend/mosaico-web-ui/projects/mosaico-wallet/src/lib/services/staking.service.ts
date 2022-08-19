import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { StakingPair } from '../models/staking-pair';
import { DistributeCommand, StakeCommand, StakeMetamaskCommand, UpdateStakingRegulationCommand } from '../commands';
import { RewardEstimateResponse, StakingStatisticsResponse, WalletStakesResponse } from '../responses';

@Injectable({
    providedIn: 'root'
})
export class StakingService {
    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public getPairs(): Observable<SuccessResponse<StakingPair[]>> {
        return this.http.get<SuccessResponse<StakingPair[]>>(`${this.baseUrl}/core/api/staking/pairs`,  { headers: DefaultHeaders });
    }

    public stake(id: string, command: StakeCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}`,  JSON.stringify(command), { headers: DefaultHeaders });
    }

    public getStakes(): Observable<SuccessResponse<WalletStakesResponse>> {
        return this.http.get<SuccessResponse<WalletStakesResponse>>(`${this.baseUrl}/core/api/staking`,  { headers: DefaultHeaders });
    }

    public getStakingStatistics(): Observable<SuccessResponse<StakingStatisticsResponse>> {
        return this.http.get<SuccessResponse<StakingStatisticsResponse>>(`${this.baseUrl}/core/api/staking/statistics`,  { headers: DefaultHeaders });
    }

    public withdraw(id: string): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}/withdrawal`,  null, { headers: DefaultHeaders });
    }

    public claimReward(id: string): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}/reward`,  null, { headers: DefaultHeaders });
    }

    public distribute(id: string, command: DistributeCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}/distribution`,  JSON.stringify(command), { headers: DefaultHeaders });
    }

    public getRewardEstimate(id: string): Observable<SuccessResponse<RewardEstimateResponse>> {
        return this.http.get<SuccessResponse<RewardEstimateResponse>>(`${this.baseUrl}/core/api/staking/${id}/estimate`,  { headers: DefaultHeaders });
    }

    public stakeMetamask(id: string, command: StakeMetamaskCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}/metamask`,  JSON.stringify(command), { headers: DefaultHeaders });
    }

    public withdrawMetamask(id: string, wallet: string, transactionHash: string): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}/metamask/withdrawal`, JSON.stringify({wallet, transactionHash}), { headers: DefaultHeaders });
    }

    public updateRegulation(id: string, command: UpdateStakingRegulationCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/staking/${id}/regulations`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public updateTerms(id: string, tokenId: string, file: File, language: string = 'en'): Observable<SuccessResponse<string>> {
        const formData = new FormData();
        formData.append('file', file, file.name);
        const headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/staking/${id}/regulations`, formData, { headers, params: {tokenId, language} });
    }
}