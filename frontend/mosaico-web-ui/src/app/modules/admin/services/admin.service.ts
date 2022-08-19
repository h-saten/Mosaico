import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, } from 'rxjs';
import { RequestList } from '../models';
import { Store } from '@ngrx/store';
import { selectUserInformation } from '../../user-management/store';
import { ConfigService, DefaultHeaders, PaginationResponse, SuccessResponse } from 'mosaico-base';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private currentUserId: string = '';
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService, store: Store) {
    this.baseUrl = configService.getConfig().gatewayUrl;
    store.select(selectUserInformation).subscribe((user) => {
      if (user) {
        this.currentUserId = user.id;
      }
    });
  }
  getDeletionRequests(skip: number = 0, take: number = 10): Observable<SuccessResponse<PaginationResponse<RequestList>>> {
    return this.http.get<SuccessResponse<PaginationResponse<RequestList>>>(`${this.baseUrl}/id/api/users/deletionrequests?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  restoreAccount(id: string): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/id/api/users/${id}/restore`,  { headers: DefaultHeaders });
  }

}
