import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, PaginationResponse, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { ProjectsList } from '../models/projects-list';
import { Observable } from 'rxjs';
import { UpdateProjectVisibilityCommand } from '../commands';

@Injectable({
    providedIn: 'root'
})
export class AdminProjectService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getProjects(skip: number = 0, take: number = 10, status: string = '', freeText: string = ''): Observable<SuccessResponse<PaginationResponse<ProjectsList>>> {
        return this.http.get<SuccessResponse<PaginationResponse<ProjectsList>>>(`${this.baseUrl}/core/api/admin/projects?skip=${skip}&take=${take}&status=${status}&freeTextSearch=${freeText}`, { headers: DefaultHeaders });
    }

    approve(id: string): Observable<SuccessResponse<boolean>> {
        return this.http.put<SuccessResponse<boolean>>(`${this.baseUrl}/core/api/admin/projects/${id}/status/acceptance`, null, { headers: DefaultHeaders });
    }

    updateVisibility(id: string, command: UpdateProjectVisibilityCommand): Observable<SuccessResponse<string>> {
        return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/admin/projects/${id}/visibility`, JSON.stringify(command), { headers: DefaultHeaders });
    }

}
