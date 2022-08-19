import { Operation } from '../models/operation';
import { SuccessResponse, PaginationResponse, ConfigService, DefaultHeaders } from 'mosaico-base';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})

export class OperationsService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getUserOperations(skip: number = 0, take: number = 10): Observable<SuccessResponse<PaginationResponse<Operation>>> {
        return this.http.get<SuccessResponse<PaginationResponse<Operation>>>(`${this.baseUrl}/core/api/users/operations`, { headers: DefaultHeaders, params: {skip, take} });
    }
}