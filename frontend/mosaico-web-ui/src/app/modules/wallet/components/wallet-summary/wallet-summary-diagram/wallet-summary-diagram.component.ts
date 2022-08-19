import { Component, Input, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { ApexChart, ApexDataLabels, ApexLegend, ApexNonAxisChartSeries, ApexResponsive, ApexTheme, ChartComponent } from 'ng-apexcharts';
import { Token, WalletBalance } from 'mosaico-wallet';
import { Store } from '@ngrx/store';
import { selectWalletTokenBalance } from '../../../store/wallet.selectors';
import { SubSink } from 'subsink';

export type DiagramOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  legend: ApexLegend;
  theme: ApexTheme;
  dataLabels: ApexDataLabels;
};

@Component({
  selector: 'app-wallet-summary-diagram',
  templateUrl: './wallet-summary-diagram.component.html',
  styleUrls: ['./wallet-summary-diagram.component.scss']
})

export class WalletSummaryDiagramComponent implements OnInit, OnDestroy {
  isLoaded = false;
  hasData = false;
  @ViewChild("chart") chart: ChartComponent;
  public diagramOptions: Partial<DiagramOptions>;
  subs = new SubSink();

  wallet: WalletBalance;

  constructor(private store: Store) {
    this.diagramOptions = {
      chart: {
        type: "pie",
        background: 'none'
      },
      theme: {
        monochrome: {
          enabled: true,
          color: '#0063F5'
        }
      },
      dataLabels: {
        style: {
          fontFamily: "Poppins",
          fontWeight: 'normal'
        },
        background: {
          enabled: true,
          foreColor: '#000',
          padding: 4,
          borderRadius: 2,
          borderWidth: 1,
          borderColor: '#000',
          opacity: 0.9,
        },
        dropShadow: {
          enabled: false
        }
      },
      legend: {
        position: "bottom"
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 200,
            },
            legend: {
              position: "bottom"
            }
          }
        }
      ]
    };
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.store.select(selectWalletTokenBalance).subscribe((b) => {
      this.wallet = b;
      if (this.wallet && !this.isLoaded) {
        const tokens = this.wallet.tokens?.slice(0, 4);
        if (tokens && tokens.length > 0) {
          this.diagramOptions = {
            ...this.diagramOptions,
            series: tokens.map((t) => t.totalAssetValue),
            labels: tokens.map((t) => t.symbol)
          };
          this.diagramOptions.series?.forEach((s) => {
            if(this.hasData === true) {
              return;
            }
            if(s > 0) {
              this.hasData = true;
            }
          });
        }
        this.isLoaded = true;
      }
    });
  }
}
