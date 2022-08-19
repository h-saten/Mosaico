import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, SuccessResponse } from 'mosaico-base';
import { Observable, of } from 'rxjs';
import { SystemWallet } from '../models';

@Injectable({
    providedIn: 'root'
})
export class SystemWalletService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    getSystemWallets(): Observable<SuccessResponse<SystemWallet[]>> {
        return of({
            data: [
                {
                    name: 'Mosaico Wallet',
                    logoUrl: '/assets/media/logos/mosaico_sygnet.png',
                    key: 'MOSAICO_WALLET',
                    disabled: false
                },
                {
                    name: 'Metamask',
                    logoUrl: '/assets/media/icons/metamask.svg',
                    key: 'METAMASK',
                    disabled: true
                }
            ],
            ok: true
        });
    }
}