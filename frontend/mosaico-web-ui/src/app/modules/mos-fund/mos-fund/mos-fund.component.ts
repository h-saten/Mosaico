import { Component, OnInit, ViewChild } from '@angular/core';
import { ExchangeRateService, TokenHistoryRecord, TokenPriceHistory, VentureFundHistoryResponse, VentureFundService } from 'mosaico-wallet';

import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexStroke,
  ApexYAxis,
  ApexTitleSubtitle,
  ApexLegend
} from "ng-apexcharts";

import { series } from "./series-data";
import { SubSink } from 'subsink';
import { CounterService, GetKPIResponse, TranslationService } from 'mosaico-base';
import { zip } from 'rxjs';
import { SuccessResponse } from '../../../../../projects/mosaico-base/src/lib/utils/success-response';
import { TranslateService } from '@ngx-translate/core';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  yaxis: ApexYAxis;
  title: ApexTitleSubtitle;
  labels: string[];
  legend: ApexLegend;
  subtitle: ApexTitleSubtitle;
};

export interface MosaicoPortfolio {
  tokenAmount: number;
  tokenPrice: number;
  totalValue: number;
  projectName: string;
  tokenSymbol: string;
  isStakingEnabled: boolean;
  tokenLogoUrl: string;
  tokenPriceChanges: number[];
}

@Component({
  selector: 'app-mos-fund',
  templateUrl: './mos-fund.component.html',
  styleUrls: ['./mos-fund.component.scss']
})
export class MosFundComponent implements OnInit {
  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  subs = new SubSink();
  isLoaded = false;
  totalValue = 0;
  counters: GetKPIResponse;

  lastUpdatedAt?: string;
  chartLabels: any = {};
  chartSeries: any = {};
  culture: string = 'en';

  mosToken: TokenPriceHistory;
  portfolio: TokenPriceHistory[] = [];

  constructor(private service: VentureFundService, private counterService: CounterService, private exchangeService: ExchangeRateService,
    private translationService: TranslateService) {
    this.chartOptions = {
      chart: {
        type: "area",
        height: 245,
        zoom: {
          enabled: false
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        type: "datetime"
      },
      yaxis: {
        opposite: false
      },
      legend: {
        horizontalAlign: "left"
      }
    };
  }

  ngOnInit(): void {
    this.loadData();
    this.subs.sink = this.translationService.onLangChange.subscribe((n) => {
      this.culture = n?.lang;
    });
  }

  private loadData(): void {
    if(!this.isLoaded) {
      this.subs.sink = zip(this.service.getFundHistory("MOS"), this.counterService.getKPIs(), this.exchangeService.getHistoricalRate("MOS")).subscribe((responses) => {
        this.setAssets(responses[0]);
        this.setStats(responses[1]);
        this.setHistoricalData(responses[2]);
        this.isLoaded = true;
      }, (error) => this.isLoaded = false);
    }
  }

  private setAssets(res: SuccessResponse<VentureFundHistoryResponse>): void {
    this.portfolio = res?.data?.tokens;
    this.isLoaded = true;
    this.totalValue = res?.data?.totalAssetValue;
    this.portfolio?.forEach((t) => {
      this.chartLabels[t.tokenSymbol] = this.getChartLabels(t);
      this.chartSeries[t.tokenSymbol] = this.getChartSeries(t);
    });
    this.lastUpdatedAt = res?.data?.lastUpdatedAt;
  }

  private setStats(res: SuccessResponse<GetKPIResponse>): void {
    this.counters = res?.data;
  }

  private setHistoricalData(res: SuccessResponse<TokenPriceHistory>): void {
    if(res?.data?.records) {
      this.mosToken = res.data;
      this.chartOptions.labels = this.getChartLabels(res?.data);
      this.chartOptions.series = this.getChartSeries(res?.data);
    }
  }

  public getChartSeries(i: TokenPriceHistory): any {
    return [
      {
        name: `${i.tokenSymbol}/USDT`,
        data: i?.records.map((r) => r.rate.toFixed(4))
      }
    ];
  }

  public getChartLabels(i: TokenPriceHistory): any {
    return i?.records.map((r) => r.date);
  }

}
