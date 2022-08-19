import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService, SuccessResponse } from 'mosaico-base';

@Injectable({ 
    providedIn: 'root'
})

export class TokenLogoService {

    private baseUrl: string = '';

    constructor(
        private http: HttpClient,
        private config: ConfigService
    ) {
        this.baseUrl = config.getConfig().gatewayUrl;
    }

    uploadTokenLogo(tokenId : string, fileToUpload: File): Observable<SuccessResponse<string>> {
        const endpoint = `${this.baseUrl}/core/api/tokens/${tokenId}/logo`;
        const formData: FormData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.http.put<SuccessResponse<string>>(endpoint, formData);
    }
} 