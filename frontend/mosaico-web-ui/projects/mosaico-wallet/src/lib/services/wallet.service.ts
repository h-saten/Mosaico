import {WalletBalance, WalletTokenBalance} from './../models';
import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {GetWalletVestingResponse} from '../responses/get-wallet-vesting.response';
import {GetWalletStakingsResponse} from "../responses/get-wallet-stakings.response";
import {
  GetTokenStakingsResponse,
  GetWalletHistoryResponse,
  WalletNativeBalanceResponse,
  WalletStagePurchaseSummaryResponse,
  WalletStakingWithdrawalsResponse
} from "../responses";
import {ConfigService, DefaultHeaders, SuccessResponse} from 'mosaico-base';
import {DepositStakeCommand} from 'mosaico-project';
import {SendWalletCurrencyCommand, SendWalletTokensCommand, WithdrawWalletCurrencyCommand} from 'mosaico-wallet';
import {GetWalletPackagesResponse} from '../responses/get-wallet-packages.response';

@Injectable({
  providedIn: 'root'
})
export class WalletService {
    private readonly baseUrl: string = "";

    constructor(private http: HttpClient, configService: ConfigService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public getTokenBalance(userId: string, network: string): Observable<SuccessResponse<WalletBalance>> {
        return this.http.get<SuccessResponse<WalletBalance>>(`${this.baseUrl}/core/api/wallets/${userId}/${network}/tokens`, { headers: DefaultHeaders});
    }

    public getUserTokenBalance(tokenId: string): Observable<SuccessResponse<WalletTokenBalance>> {
        return this.http.get<SuccessResponse<WalletTokenBalance>>(`${this.baseUrl}/core/api/wallets/token/${tokenId}/balance`, { headers: DefaultHeaders});
    }

    public getPackages(wallet: string, network: string): Observable<SuccessResponse<GetWalletPackagesResponse>>{
      return this.http.get<SuccessResponse<GetWalletPackagesResponse>>(`${this.baseUrl}/core/api/wallets/${wallet}/${network}/packages`, { headers: DefaultHeaders});
    }

    public getWalletStageSummary(userId: string, network: string, stageId: string): Observable<SuccessResponse<WalletStagePurchaseSummaryResponse>> {
      return this.http.get<SuccessResponse<WalletStagePurchaseSummaryResponse>>(`${this.baseUrl}/core/api/wallets/${userId}/${network}/stage/${stageId}`, { headers: DefaultHeaders});
    }

    public sendTokens(wallet: string, network: string, command: SendWalletTokensCommand): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/wallets/${wallet}/${network}/tokens/transaction`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public sendCurrency(wallet: string, network: string, command: SendWalletCurrencyCommand): Observable<SuccessResponse<any>> {
      return this.http.post<SuccessResponse<any>>(`${this.baseUrl}/core/api/wallets/${wallet}/${network}/currency/transaction`, JSON.stringify(command), { headers: DefaultHeaders});
    }

    public walletHistory(wallet: string, network: string): Observable<SuccessResponse<GetWalletHistoryResponse>>{
      return this.http.get<SuccessResponse<GetWalletHistoryResponse>>(`${this.baseUrl}/core/api/wallets/${wallet}/${network}/history`, { headers: DefaultHeaders});
    }

    getBalance(userId: string, network: string): Observable<SuccessResponse<WalletNativeBalanceResponse>> {
      return this.http.get<SuccessResponse<WalletNativeBalanceResponse>>(`${this.baseUrl}/core/api/wallets/${userId}/${network}/balance`, { headers: DefaultHeaders });
    }

    getWalletVesting(userId: string, network: string): Observable<SuccessResponse<GetWalletVestingResponse>> {
      return this.http.get<SuccessResponse<GetWalletVestingResponse>>(`${this.baseUrl}/core/api/wallets/${userId}/${network}/vestings`, { headers: DefaultHeaders });
    }
}
