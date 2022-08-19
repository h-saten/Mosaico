import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { SendInvitationsCommand  } from '../commands';
import { GetInvitationsResponse } from '../responses';

@Injectable({
  providedIn: 'root'
})
export class InvitationsService {
  private baseUrl = '';

  constructor(
    private http: HttpClient,
    private configService: ConfigService
  ) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  sendInvitation(command: SendInvitationsCommand): Observable<SuccessResponse<object>> {
    return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/invitations`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getInvitations(): Observable<SuccessResponse<GetInvitationsResponse>> {
    return this.http.get<SuccessResponse<GetInvitationsResponse>>(`${this.baseUrl}/core/api/invitations`, { headers: DefaultHeaders });
  }

  acceptInvitations(id: string): Observable<SuccessResponse<boolean>> {
    return this.http.put<SuccessResponse<boolean>>(`${this.baseUrl}/core/api/invitations/${id}`, null, { headers: DefaultHeaders });
  }

}
