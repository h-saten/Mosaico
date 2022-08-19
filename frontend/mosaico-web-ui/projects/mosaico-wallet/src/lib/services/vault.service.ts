import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from 'rxjs';
import { SendVaultTokensCommand } from '../commands/send-vault-tokens.command';

@Injectable({
    providedIn: 'root'
})
export class VaultService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    createVault(tokenId: string): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/vaults`, JSON.stringify({tokenId}), { headers: DefaultHeaders });
    }
    
    createDeposit(vaultId: string, distributionId: string): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/vaults/${vaultId}/deposit`, JSON.stringify({tokenDistributionId:distributionId}), { headers: DefaultHeaders });
    }

    sendVaultTokens(id: string, command: SendVaultTokensCommand): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/vaults/${id}/transfer`, JSON.stringify(command), { headers: DefaultHeaders });
    }
}