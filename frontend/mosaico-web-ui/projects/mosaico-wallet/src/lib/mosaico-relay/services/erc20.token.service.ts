import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RelayHeaders } from "../constants";
import { ConfigService, SuccessResponse } from 'mosaico-base';
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class ERC20TokenService {

    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, private configService: ConfigService) {
        this.baseUrl = configService.getConfig().relayUrl;
    }

    getTokenAbi(version: string = 'v1'): Observable<SuccessResponse<any>> {
        return this.http.get<SuccessResponse<any>>(`${this.baseUrl}/api/${version}/tokens/erc20/abi`, { headers: RelayHeaders });
    }
}