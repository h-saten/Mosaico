import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ConfigService, SuccessResponse, DefaultHeaders } from 'mosaico-base';
import { Observable } from "rxjs";
import { CreateBankTransferCommand, InitiateTransactionCommand, ValidateOrderCommand } from '../commands';
import { BankPaymentDetails, Transaction } from "../models";
import { BinanceTransactionResponse, CCQuoteResponse, OrderStatusResponse, OrderValidationResponse } from "../responses";

@Injectable({
    providedIn: 'root'
})
export class OrdersService {
    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public initOrder(projectId: string, command: InitiateTransactionCommand): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${projectId}/orders`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public initRampOrder(projectId: string, command: any): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/ramp`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public initTransakOrder(projectId: string, command: any): Observable<SuccessResponse<any>> {
        return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/transak`, JSON.stringify(command), { headers: DefaultHeaders });
    }
    
    public getQuote(projectId: string, type: string): Observable<SuccessResponse<CCQuoteResponse>> {
        return this.http.get<SuccessResponse<CCQuoteResponse>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/quote?type=${type}`, { headers: DefaultHeaders });
    }

    public getStatus(id: string): Observable<SuccessResponse<OrderStatusResponse>> {
        return this.http.get<SuccessResponse<OrderStatusResponse>>(`${this.baseUrl}/core/api/orders/${id}/status`, { headers: DefaultHeaders });
    }

    public initBankTransfer(projectId: string, command: CreateBankTransferCommand): Observable<SuccessResponse<BankPaymentDetails>> {
        return this.http.post<SuccessResponse<BankPaymentDetails>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/bankTransfer`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public confirmBankTransfer(projectId: string, id: string): Observable<SuccessResponse<any>> {
        return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/${id}`, null, { headers: DefaultHeaders });
    }

    public getTransaction(projectId: string, id: string): Observable<SuccessResponse<Transaction>> {
        return this.http.get<SuccessResponse<Transaction>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/${id}`, { headers: DefaultHeaders });
    }

    public initMetamask(projectId: string, command: any): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/metamask`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public validate(projectId: string, command: ValidateOrderCommand): Observable<SuccessResponse<OrderValidationResponse>> {
        return this.http.post<SuccessResponse<OrderValidationResponse>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/validation`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public initMosaicoWallet(projectId: string, command: any): Observable<SuccessResponse<string>> {
        return this.http.post<SuccessResponse<string>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/mosaico-wallet`, JSON.stringify(command), { headers: DefaultHeaders });
    }

    public initBinance(projectId: string, command: any): Observable<SuccessResponse<BinanceTransactionResponse>> {
        return this.http.post<SuccessResponse<BinanceTransactionResponse>>(`${this.baseUrl}/core/api/projects/${projectId}/orders/binance`, JSON.stringify(command), { headers: DefaultHeaders });
    }
}
