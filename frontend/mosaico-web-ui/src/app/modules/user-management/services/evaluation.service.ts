import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse } from 'mosaico-base';
import { Observable, of } from 'rxjs';
import { EvaluationQuestion } from '../models/evaluation-question';

@Injectable({
    providedIn: 'root'
  })
export class EvaluationService {
    private baseUrl = "";

    constructor(private http: HttpClient, configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public getQuestions(): Observable<SuccessResponse<EvaluationQuestion[]>> {
        return of({
            ok: true,
            data: [
                {
                    title: 'BLOCKCHAIN',
                    key: 'NCcJI3VQXZ',
                    response: false
                },
                {
                    title: 'TOKEN_ASSETS',
                    key: 'pMOKOc0b2g',
                    response: false
                },
                {
                    title: 'INVESTED_BEFORE',
                    key: 'HkrGZMKw2D',
                    response: false
                },
                {
                    title: 'INVESTMENT_RISKS',
                    key: 'mmfsbTova7',
                    response: false
                }
            ]
        });
    }
};