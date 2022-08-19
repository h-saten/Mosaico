import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ConfigService} from './config.service';
import {DefaultHeaders, SuccessResponse} from '../utils';
import {Observable} from 'rxjs';
import {SendMessageCommand} from "../commands";

@Injectable({
    providedIn: 'root'
})
export class ContactService {
    private baseUrl = '';

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    sendMessage(command: SendMessageCommand): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/contact`, JSON.stringify(command), { headers: DefaultHeaders });
    }

}
