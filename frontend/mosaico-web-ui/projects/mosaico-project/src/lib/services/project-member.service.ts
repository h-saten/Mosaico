import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { AcceptProjectInvitationCommand, AddProjectMemberCommand, UpdateProjectMemberCommand } from '../commands';
import { GetMembersResponse } from '../responses';

@Injectable({
    providedIn: 'root'
})
export class ProjectMemberService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    addProjectMember(id: string, command: AddProjectMemberCommand): Observable<SuccessResponse<object>> {
        return this.http.post<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${id}/members`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    deleteProjectMember(id: string, memberId: string): Observable<SuccessResponse<object>> {
        return this.http.delete<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${id}/members/${memberId}`, { headers: DefaultHeaders });
    }

    acceptInvitation(command: AcceptProjectInvitationCommand): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/invitations`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    getMembers(id: string): Observable<SuccessResponse<GetMembersResponse>> {
        return this.http.get<SuccessResponse<GetMembersResponse>>(`${this.baseUrl}/core/api/projects/${id}/members`, { headers: DefaultHeaders });
    }

    updateMemberRole(id: string, memberId: string, command: UpdateProjectMemberCommand) : Observable<SuccessResponse<object>> {
        return this.http.put<SuccessResponse<object>>(`${this.baseUrl}/core/api/projects/${id}/members/${memberId}`, JSON.stringify(command), { headers: DefaultHeaders });
    }
}
