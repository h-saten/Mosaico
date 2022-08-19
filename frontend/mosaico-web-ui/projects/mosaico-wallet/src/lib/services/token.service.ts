import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {ConfigService, DefaultHeaders, SuccessResponse} from 'mosaico-base';
import {Observable} from 'rxjs';
import {Token} from '../models';
import {
  BurnTokenCommand,
  CreateTokenCommand,
  EnableDeflationCommand,
  EnableStakingCommand,
  EnableTokenVestingCommand,
  EnableVestingCommand,
  ImportTokenCommand,
  MintTokenCommand,
  UpdateTokenCommand,
  UpsertExternalExchangeCommand
} from '../commands';
import {TokenPermissions} from '../models';
import {ImportTokenDetailsResponse} from "../responses";
import { SendVaultTokensCommand } from '../commands/send-vault-tokens.command';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  getToken(id: string): Observable<SuccessResponse<Token>> {
    return this.http.get<SuccessResponse<Token>>(`${this.baseUrl}/core/api/tokens/${id}`, { headers: DefaultHeaders });
  }

  getCompanyTokens(companyId: string): Observable<SuccessResponse<Token[]>> {
    return this.http.get<SuccessResponse<Token[]>>(`${this.baseUrl}/core/api/tokens?companyId=${companyId}`, { headers: DefaultHeaders });
  }

  createToken(command: CreateTokenCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/tokens`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  importToken(command: ImportTokenCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/tokens/import`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  upsertExternalExchange(id: string, command: UpsertExternalExchangeCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/exchange`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  deleteExternalExchange(id: string, externalExchangeId: string): Observable<SuccessResponse<any>> {
    return this.http.delete<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/exchange/${externalExchangeId}`, { headers: DefaultHeaders });
  }

  preValidateTokenCreation(command: CreateTokenCommand): Observable<SuccessResponse<string>> {
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/tokens/prevalidate`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getTokenPermissions(id: string): Observable<SuccessResponse<TokenPermissions>> {
    return this.http.get<SuccessResponse<TokenPermissions>>(`${this.baseUrl}/core/api/tokens/${id}/permissions`, { headers: DefaultHeaders });
  }

  updateTokenLogo(file: File, id: string): Observable<SuccessResponse<string>> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/tokens/${id}/logo`, formData, { headers });
  }

  mint(id: string, command: MintTokenCommand): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/mint`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  burn(id: string, command: BurnTokenCommand): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/burn`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  update(id: string, command: UpdateTokenCommand): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  deploy(id: string): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/deployment`, null, { headers: DefaultHeaders });
  }

  getTokenDetails(chain: string, address: string): Observable<SuccessResponse<ImportTokenDetailsResponse>> {
    return this.http.get<SuccessResponse<ImportTokenDetailsResponse>>(`${this.baseUrl}/core/api/tokens/${chain}/${address}/details`, { headers: DefaultHeaders });
  }

  enableStaking(id: string, command: EnableStakingCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/stakings`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  enableDeflation(id: string, command: EnableDeflationCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/deflation`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  enableVesting(id: string, command: EnableTokenVestingCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/tokens/${id}/vesting`, JSON.stringify(command), { headers: DefaultHeaders });
  }
}
