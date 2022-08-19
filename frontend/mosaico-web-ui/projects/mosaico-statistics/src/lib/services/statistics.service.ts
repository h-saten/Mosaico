import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable,} from 'rxjs';
import {
  DailyRaisedCapitalResponse,
  RaisedCapitalByCurrencyResponse,
  StatisticsSummaryResponse,
  TopInvestorsResponse,
} from '../responses';
import {Store} from '@ngrx/store';
import {ConfigService, DefaultHeaders, SuccessResponse} from 'mosaico-base';
import {DailyVisitsStatisticsResponse} from "../responses/daily-visits-statistics.response";

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService, store: Store) {
    this.baseUrl = configService.getConfig().gatewayUrl;
  }

  dailyVisitStatistics(id: string): Observable<SuccessResponse<DailyVisitsStatisticsResponse>> {
    return this.http.get<SuccessResponse<DailyVisitsStatisticsResponse>>(`${this.baseUrl}/core/api/projects/${id}/statistics/visits/daily`, { headers: DefaultHeaders });
  }

  topInvestors(id: string): Observable<SuccessResponse<TopInvestorsResponse>> {
    return this.http.get<SuccessResponse<TopInvestorsResponse>>(`${this.baseUrl}/core/api/projects/${id}/statistics/sale/top-investors`, { headers: DefaultHeaders });
  }

  dailyRaisedCapital(id: string, monthsAgo = 0): Observable<SuccessResponse<DailyRaisedCapitalResponse>> {
    return this.http.get<SuccessResponse<DailyRaisedCapitalResponse>>(`${this.baseUrl}/core/api/projects/${id}/statistics/sale/daily?monthsAgo=${monthsAgo}`, { headers: DefaultHeaders });
  }

  raisedFundsByCurrency(id: string): Observable<SuccessResponse<RaisedCapitalByCurrencyResponse>> {
    return this.http.get<SuccessResponse<RaisedCapitalByCurrencyResponse>>(`${this.baseUrl}/core/api/projects/${id}/statistics/sale/by-currency`, { headers: DefaultHeaders });
  }

  summary(id: string): Observable<SuccessResponse<StatisticsSummaryResponse>> {
    return this.http.get<SuccessResponse<StatisticsSummaryResponse>>(`${this.baseUrl}/core/api/projects/${id}/statistics/summary`, { headers: DefaultHeaders });
  }

}
