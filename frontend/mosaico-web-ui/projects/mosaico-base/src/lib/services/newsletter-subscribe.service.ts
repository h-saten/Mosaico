import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable} from 'rxjs';
import {SuccessResponse,DefaultHeaders} from '../utils';
import {ConfigService} from './config.service';
import {NewsLetterSubscribe} from '../models/newsletter-subscribe';

@Injectable({
    providedIn: 'root'
})
export class NewsLetterSubscribeService{
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public subscribeNewsLetter(command: NewsLetterSubscribe): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/id/api/NewsletterSubscriber/SubscribeToNewsletter`, JSON.stringify(command), { headers: DefaultHeaders});
    }
}
