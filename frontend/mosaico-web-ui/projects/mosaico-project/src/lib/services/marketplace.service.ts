import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService, DefaultHeaders, PaginationResponse, SuccessResponse } from 'mosaico-base';
import { Observable } from 'rxjs';
import { ProjectsList } from '../models';

@Injectable({
  providedIn: 'root'
})
export class MarketplaceService {
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  getUserProjects(skip: number = 0, take: number = 10, userId: string = ''): Observable<SuccessResponse<PaginationResponse<ProjectsList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<ProjectsList>>>(`${this.baseUrl}/core/api/self/projects?skip=${skip}&take=${take}&userId=${userId}`, { headers: DefaultHeaders });
  }

  getProjects(skip: number = 0, take: number = 10, status: string = '', textSearch: string = ''): Observable<SuccessResponse<PaginationResponse<ProjectsList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<ProjectsList>>>(`${this.baseUrl}/core/api/projects?skip=${skip}&take=${take}&status=${status}&textSearch=${textSearch}`, { headers: DefaultHeaders });
  }

  getProjectsForLanding(params: any): Observable<SuccessResponse<PaginationResponse<ProjectsList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<ProjectsList>>>(`${this.baseUrl}/core/api/projects`, { headers: DefaultHeaders, params });
  }

}
