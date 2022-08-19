import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ChartOptions, DiagramOptions } from 'src/app/modules/wallet';
import { SubSink } from 'subsink';
import { selectCompanyWalletBalance } from '../../store/business.selectors';

@Component({
  selector: 'app-company-wallet-statistics',
  templateUrl: './company-wallet-statistics.component.html',
  styleUrls: ['./company-wallet-statistics.component.scss']
})
export class CompanyWalletStatisticsComponent implements OnInit, OnDestroy {
  public diagramOptions: Partial<ChartOptions>;
  public diagramOptions2: Partial<DiagramOptions>;
  subs = new SubSink();
  balanceDistributionHasData = false;
  historyHasData = false;

  constructor(private store: Store) { 
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
    this.diagramOptions2 = {
      series: [62, 21, 21, 24],
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
        dropShadow: {
          enabled: false
        }
      },
      labels: ["ETH", "BCN", "XRP", "XMR"],
      legend: {
        position: "bottom"
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 200
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
    this.subs.sink = this.store.select(selectCompanyWalletBalance).subscribe((res) => {
      if(res) {
        this.balanceDistributionHasData = false;
        const tokens = res.tokens?.slice(0, 4);
        if (tokens && tokens.length > 0) {
          this.diagramOptions2 = {
            ...this.diagramOptions2,
            series: tokens.map((t) => t.totalAssetValue),
            labels: tokens.map((t) => t.symbol)
          };
          this.diagramOptions2.series?.forEach((s) => {
            if(this.balanceDistributionHasData === true) {
              return;
            }
            if(s > 0) {
              this.balanceDistributionHasData = true;
            }
          });
        }
      }
    });
  }

}
