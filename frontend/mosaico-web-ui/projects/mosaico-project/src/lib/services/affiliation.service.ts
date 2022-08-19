import { Injectable } from '@angular/core';
import { ConfigService, SuccessResponse, DefaultHeaders, PaginationResponse } from 'mosaico-base';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddAffiliationReferenceCommand, UpdateAffiliationCommand, UpdatePartnerCommand } from '../commands/affiliation';
import { Affiliation, AffiliationPartner, Partner } from '../models';
import { GetUserAffiliationResponse } from '../responses';

@Injectable({
    providedIn: 'root'
})
export class AffiliationService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    updateProjectAffiliation(id: string, command: UpdateAffiliationCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/affiliation`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    getAffiliation(id: string): Observable<SuccessResponse<Affiliation>> {
        return this.http.get<SuccessResponse<Affiliation>>(`${this.baseUrl}/core/api/projects/${id}/affiliation`, { headers: DefaultHeaders });
    }

    getPartners(id: string, skip: number, take: number): Observable<SuccessResponse<PaginationResponse<AffiliationPartner>>> {
        return this.http.get<SuccessResponse<PaginationResponse<AffiliationPartner>>>(`${this.baseUrl}/core/api/projects/${id}/affiliation/partners`, { headers: DefaultHeaders, params: {skip, take} });
    }

    updatePartner(id: string, partnerId: string, command: UpdatePartnerCommand): Observable<SuccessResponse<any>> {
        return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/affiliation/partners/${partnerId}`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    enablePartner(id: string, partnerId: string): Observable<SuccessResponse<any>> {
        return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/affiliation/partners/${partnerId}/enable`, null, { headers: DefaultHeaders });
    }

    disablePartner(id: string, partnerId: string): Observable<SuccessResponse<any>> {
        return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${id}/affiliation/partners/${partnerId}/disable`, null, { headers: DefaultHeaders });
    }

    getUserAffiliation(): Observable<SuccessResponse<GetUserAffiliationResponse>> {
        return this.http.get<SuccessResponse<GetUserAffiliationResponse>>(`${this.baseUrl}/core/api/affiliations/self`, { headers: DefaultHeaders });
    }

    addReference(command: AddAffiliationReferenceCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/affiliations/reference`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public addAffiliationPartner(id: string, email: string): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${id}/affiliation/partners`, JSON.stringify({email}), { headers: DefaultHeaders });
    }
}