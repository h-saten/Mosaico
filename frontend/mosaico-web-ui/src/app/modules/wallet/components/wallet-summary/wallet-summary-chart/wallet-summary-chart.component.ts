import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexStroke,
  ApexTheme,
  ApexLegend
} from "ng-apexcharts";
import { GetWalletHistoryResponse, WalletService } from 'mosaico-wallet';
import { selectWallet, selectUserWallet } from '../../../store/wallet.selectors';
import { ApexYAxis } from 'ng-apexcharts';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
  theme: ApexTheme;
  legend: ApexLegend;
};

@Component({
  selector: 'app-wallet-summary-chart',
  templateUrl: './wallet-summary-chart.component.html',
  styleUrls: ['./wallet-summary-chart.component.scss']
})

export class WalletSummaryChartComponent implements OnInit {

  @ViewChild("chart") chart: ChartComponent;
  public diagramOptions: Partial<ChartOptions>;
  sub: SubSink = new SubSink();
  isLoaded = false;
  hasData = false;

  constructor(private walletService: WalletService, private store: Store) {
    this.diagramOptions = {
      theme: {
        monochrome: {
          enabled: true,
          color: '#0063F5'
        }
      },
      chart: {
        type: "area",
        height: "200px",
        background: 'none',
        zoom: {
          enabled: false
        }
      },
      dataLabels: {
        enabled: false,
        style: {
          fontFamily: "Poppins",
          fontWeight: 'normal'
        },
        dropShadow: {
          enabled: false
        }
      },
      stroke: {
        curve: "smooth"
      },
      tooltip: {
        x: {
          format: "dd/MM/yy HH:mm"
        }
      },
      legend: {
        horizontalAlign: "left"
      }
    };
  }

  ngOnInit(): void {
    this.sub.sink = this.store.select(selectUserWallet).subscribe((res) => {
      if (res && !this.isLoaded) {
        this.sub.sink = this.walletService.walletHistory(res.address, res.network).subscribe((response) => {
          this.fillInChartData(response?.data);
          this.isLoaded = true;
        }, (error) => { this.isLoaded = true; this.hasData = false; });
      }
    });
  }

  private fillInChartData(response: GetWalletHistoryResponse): void {
    if (response) {
      const series = response.balances?.map((b) => b.balance);
      const dates = response.balances?.map((b) => b.date);
      if (series && dates) {
        this.diagramOptions = {
          ...this.diagramOptions,
          series: [
            {
              name: "Balance",
              data: series
            }
          ],
          xaxis: {
            type: "datetime",
            categories: dates
          },
          yaxis: {
            labels: {
              formatter: (v) => {
                return v?.toFixed(2);
              }
            }
          }
        };
        series.forEach((s) => {
          if (this.hasData === true) {
            return;
          }
          if (s > 0) {
            this.hasData = true;
          }
        });
      }
    }
    else {
      this.hasData = false;
    }
  }
}
