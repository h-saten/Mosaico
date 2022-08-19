import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders, PaginationResponse } from 'mosaico-base';
import { Observable } from 'rxjs';
import { CreateAirdropCommand } from '../commands/create-airdrop.command';
import { AddAirdropParticipantCommand } from '../commands/add-airdrop-participant.command';
import { Airdrop } from '../models/airdrop';
import { AirdropParticipant } from '../models';

@Injectable({
    providedIn: 'root'
})
export class AirdropService {
    private baseUrl = '';

    constructor(
        private http: HttpClient,
        private configService: ConfigService
    ) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    create(projectId: string, command: CreateAirdropCommand): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    delete(projectId: string, airdropId: string): Observable<SuccessResponse<string>> {
        return this.http.delete<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops/${airdropId}`, { headers: DefaultHeaders });
    }

    claim(airdropId: string): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/airdrops/${airdropId}`, null, { headers: DefaultHeaders });
    }

    addParticipants(projectId: string, airdropId: string, command: AddAirdropParticipantCommand): Observable<SuccessResponse<string>> {
        return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops/${airdropId}/participants`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    getProjectAirdrops(projectId: string): Observable<SuccessResponse<Airdrop[]>> {
        return this.http.get<SuccessResponse<Airdrop[]>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops`, { headers: DefaultHeaders });
    }

    getAirdrop(airdropId: string): Observable<SuccessResponse<Airdrop>> {
        return this.http.get<SuccessResponse<Airdrop>>(`${this.baseUrl}/core/api/airdrops/${airdropId}`, { headers: DefaultHeaders });
    }

    distribute(projectId: string, airdropId: string): Observable<SuccessResponse<any>> {
        return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops/${airdropId}`, null, { headers: DefaultHeaders });
    }

    getParticipants(projectId: string, airdropId: string, take: number = 0, skip: number = 10): Observable<SuccessResponse<PaginationResponse<AirdropParticipant>>> {
        let requestData: any = { skip, take };
        return this.http.get<SuccessResponse<PaginationResponse<AirdropParticipant>>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops/${airdropId}/participants`, 
            { params: requestData, headers: DefaultHeaders });
    }

    import(projectId: string, airdropId: string, fileToUpload: File): Observable<SuccessResponse<any>> {
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        const headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/airdrops/${airdropId}/participants/import`, formData, { headers });
    }
}