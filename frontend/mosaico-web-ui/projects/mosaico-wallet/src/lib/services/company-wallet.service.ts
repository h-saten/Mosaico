import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, } from 'rxjs';
import { ConfigService, DefaultHeaders, SuccessResponse } from 'mosaico-base';
import { CompanyNativeBalanceResponse, CompanyPaymentDetailsResponse } from '../responses';
import { SendCompanyCurrencyCommand, SendCompanyTokensCommand } from '../commands';
import { CompanyWalletBalance } from '../models';

@Injectable({
  providedIn: 'root'
})
export class CompanyWalletService {

  private readonly baseUrl: string = "";

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  getCompanyWalletTokens(id: string, token?: string): Observable<SuccessResponse<CompanyWalletBalance>> {
    if(token?.length > 0){
      return this.http.get<SuccessResponse<CompanyWalletBalance>>(`${this.baseUrl}/core/api/dao/${id}/wallet?token=${token}`, { headers: DefaultHeaders });
    }
    return this.http.get<SuccessResponse<CompanyWalletBalance>>(`${this.baseUrl}/core/api/dao/${id}/wallet`, { headers: DefaultHeaders });
  }

  sendTokens(id: string, command: SendCompanyTokensCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/dao/${id}/transaction`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  sendCurrency(id: string, command: SendCompanyCurrencyCommand): Observable<SuccessResponse<any>> {
    return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/dao/${id}/currency/transaction`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  getCompanyPaymentDetails(companyId: string, network: string): Observable<SuccessResponse<CompanyPaymentDetailsResponse>> {
    return this.http.get<SuccessResponse<CompanyPaymentDetailsResponse>>(`${this.baseUrl}/core/api/dao/${companyId}/${network}/payment-details`, { headers: DefaultHeaders });
  }

  getCompanyBalance(companyId: string): Observable<SuccessResponse<CompanyNativeBalanceResponse>> {
    return this.http.get<SuccessResponse<CompanyNativeBalanceResponse>>(`${this.baseUrl}/core/api/dao/${companyId}/balance`, { headers: DefaultHeaders });
  }
}