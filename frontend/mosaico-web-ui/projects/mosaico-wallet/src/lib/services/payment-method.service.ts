import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService, DefaultHeaders, SuccessResponse } from 'mosaico-base';
import { Observable, of } from 'rxjs';
import { PaymentMethod } from '../models';
import { UpsertPaymentMethodCommand } from "../commands/upsert-payment-method.command";
import { PaymentCurrency } from "../models/payment-currency";
import { map } from "rxjs/operators";
import { UpsertPaymentCurrencyCommand } from "../commands";

export interface CurrencyLogo {
  symbol: string;
  logoUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentMethodService {

  private readonly baseUrl: string = "";

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  getPaymentMethods(): Observable<SuccessResponse<PaymentMethod[]>> {
    return of({
      data: [
        {
          id: '1',
          disabled: false,
          name: 'Mosaico Wallet',
          logoUrl: '/assets/media/logos/mosaico_sygnet.png',
          key: 'MOSAICO_WALLET'
        },
        {
          id: '2',
          disabled: false,
          name: 'Metamask',
          logoUrl: '/assets/media/icons/metamask.svg',
          key: 'METAMASK'
        },
        {
          id: '3',
          disabled: false,
          name: 'Kanga Pay',
          logoUrl: '/assets/media/logos/kanga_logo.svg',
          key: 'KANGA_EXCHANGE'
        },
        {
          id: '4',
          disabled: false,
          name: 'Visa / Master Card',
          logoUrl: '/assets/media/svg/payment-methods/visa.svg',
          key: 'CREDIT_CARD'
        },
        {
          id: '5',
          disabled: false,
          name: 'Bank transfer',
          logoUrl: '/assets/media/icons/bank_transfer.png',
          key: 'BANK_TRANSFER'
        },
        {
          id: '6',
          disabled: false,
          name: 'Binance',
          logoUrl: '/assets/media/icons/binance.png',
          key: 'BINANCE'
        }
      ],
      ok: true
    });
    //return this.http.get<SuccessResponse<ExternalExchange[]>>(`${this.baseUrl}/core/api/payment-methods`, { headers: DefaultHeaders });
  }

  upsertPaymentMethod(project: string, command: UpsertPaymentMethodCommand): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${project}/sale/payment-method`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  upsertPaymentCurrency(project: string, command: UpsertPaymentCurrencyCommand): Observable<SuccessResponse<any>> {
    return this.http.put<SuccessResponse<any>>(`${this.baseUrl}/core/api/projects/${project}/sale/payment-currency`, JSON.stringify(command), { headers: DefaultHeaders });
  }

  private static currenciesLogo(): CurrencyLogo[] {
    return [
      { symbol: 'USDT', logoUrl: '/assets/media/icons/currencies/usdt.svg' },
      { symbol: 'USDC', logoUrl: '/assets/media/icons/currencies/usdc.svg' },
      { symbol: 'ETH', logoUrl: '/assets/media/icons/currencies/eth.svg' },
      { symbol: 'MATIC', logoUrl: '/assets/media/icons/currencies/matic.svg' },
      { symbol: 'MOS', logoUrl: '/assets/media/logos/mos-token.png' }
    ];
  }

  paymentCurrencies(network: string): Observable<SuccessResponse<PaymentCurrency[]>> {
    const currenciesLogo: CurrencyLogo[] = PaymentMethodService.currenciesLogo();
    return this.http
      .get<SuccessResponse<PaymentCurrency[]>>(`${this.baseUrl}/core/api/wallets/${network}/payment-currencies`, { headers: DefaultHeaders })
      .pipe(map((response) => {
        const paymentCurrencies = response.data;
        paymentCurrencies.forEach((currency) => {
          currency.logoUrl = currenciesLogo.find(x => x.symbol == currency.ticker)?.logoUrl;
        });
        return response;
      }));
  }
}
