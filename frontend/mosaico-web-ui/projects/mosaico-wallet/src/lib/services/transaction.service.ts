
import {
  ConfirmDepositTransactionCommand,
  ConfirmPurchaseTransactionCommand,
  InitiateDepositTransactionCommand,
  InitiateKangaTransactionCommand,
  InitiateTransactionCommand
} from '../commands';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CompanyWalletTransactionsResponse,
  InitiateKangaTransactionResponse,
  ProjectInvestorQueryResponse,
  TokenKangaEstimatesResponse,
  WalletTransactionsResponse
} from "../responses";
import { ConfigService, DefaultHeaders, FilterParams, PaginationResponse, SortingParams, SuccessResponse } from 'mosaico-base';
import { ProjectTransactionsResponse } from 'mosaico-wallet';
import { Operation } from '../models/operation';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private readonly baseUrl: string = "";

  constructor(private http: HttpClient, configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  public confirmDepositTransaction(transactionId: string, command: ConfirmDepositTransactionCommand): Observable<SuccessResponse<string>> {
    return this.http.put<SuccessResponse<string>>(`${this.baseUrl}/core/api/transactions/${transactionId}/deposit`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  public getTransactions(wallet: string, network: string, skip: number = 0, take: number = 30): Observable<SuccessResponse<WalletTransactionsResponse>> {
    return this.http.get<SuccessResponse<WalletTransactionsResponse>>(`${this.baseUrl}/core/api/transactions/${wallet}/${network}?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  public getCompanyTransactions(companyWallet: string, skip: number = 0, take: number = 30): Observable<SuccessResponse<CompanyWalletTransactionsResponse>> {
    return this.http.get<SuccessResponse<CompanyWalletTransactionsResponse>>(`${this.baseUrl}/core/api/transactions/dao/${companyWallet}?skip=${skip}&take=${take}`, { headers: DefaultHeaders });
  }

  public getProjectTransactions(projectId: string, skip: number = 0, take: number = 30, from?: string, to?: string, statuses?: string[], paymentMethods?: string[], correlationId?: string): Observable<SuccessResponse<ProjectTransactionsResponse>> {
    let requestData: any = { skip, take, projectId };
    if(from && from.length > 0) { requestData = {...requestData, from}; }
    if(to && to.length > 0) { requestData = {...requestData, to}; }
    if(statuses && statuses.length > 0) { requestData = {...requestData, statuses}; }
    if(paymentMethods && paymentMethods.length > 0) { requestData = {...requestData, paymentMethods}; }
    if(correlationId && correlationId.length > 0) { requestData = { ...requestData,  correlationId};}
    return this.http.get<SuccessResponse<ProjectTransactionsResponse>>(`${this.baseUrl}/core/api/transactions/project`, { params: requestData });
  }

  public exportTransactions(projectId: string, format: string = 'CSV', from?: string, to?: string, statuses?: string[], paymentMethods?: string[]): Observable<HttpResponse<Blob>> {
    let requestData: any = { projectId, format };
    if(from && from.length > 0) { requestData = {...requestData, from}; }
    if(to && to.length > 0) { requestData = {...requestData, to}; }
    if(statuses && statuses.length > 0) { requestData = {...requestData, statuses}; }
    if(paymentMethods && paymentMethods.length > 0) { requestData = {...requestData, paymentMethods}; }
    return this.http.post(`${this.baseUrl}/core/api/transactions/export`, {}, { params: requestData, responseType: "blob", observe: 'response' });
  }

  getInvestorDetails(id: string, userId: string): Observable<SuccessResponse<ProjectInvestorQueryResponse>> {
    return this.http.get<SuccessResponse<ProjectInvestorQueryResponse>>(`${this.baseUrl}/core/api/projects/${id}/investors/${userId}`, { headers: DefaultHeaders });
  }

  public getOperations(id: string, take: number = 10, skip: number = 0): Observable<SuccessResponse<PaginationResponse<Operation>>> {
    return this.http.get<SuccessResponse<PaginationResponse<Operation>>>(`${this.baseUrl}/core/api/transactions/${id}/operations`, { headers: DefaultHeaders, params: {take, skip} });
  }

  public updateAgent(id: string, agentId: string): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/transactions/${id}/agent`, JSON.stringify({agentId}), { headers: DefaultHeaders });
  }

  public updateFee(id: string, feePercentage: number): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/transactions/${id}/fee`, JSON.stringify({feePercentage}), { headers: DefaultHeaders });
  }

  /* KANGA */
  public initKangaTransaction(command: InitiateKangaTransactionCommand): Observable<SuccessResponse<InitiateKangaTransactionResponse>> {
    return this.http.post<SuccessResponse<InitiateKangaTransactionResponse>>(`${this.baseUrl}/core/api/kanga/transaction/create`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  public tokenKangaEstimates(token: string): Observable<SuccessResponse<TokenKangaEstimatesResponse>> {
    return this.http.get<SuccessResponse<TokenKangaEstimatesResponse>>(`${this.baseUrl}/core/api/kanga/token/${token}/estimates`, { headers: DefaultHeaders });
  }
}