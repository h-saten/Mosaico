import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ProjectService, TransactionFeeResponse } from 'mosaico-project';
import { ApexAxisChartSeries, ApexChart, ApexXAxis, ApexYAxis, ApexStroke, ApexTooltip, ApexDataLabels, ApexTheme, ApexLegend, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { ChartOptions } from 'src/app/modules/wallet';
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
  selector: 'app-fees',
  templateUrl: './fees.component.html',
  styleUrls: ['./fees.component.scss']
})
export class FeesComponent implements OnInit, OnDestroy {
  @Input() projectId: string;
  subs = new SubSink();
  fee: TransactionFeeResponse;
  public diagramOptions: Partial<DiagramOptions>;
  isDataLoaded = false;

  constructor(private projectService: ProjectService) {
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
    this.subs.sink = this.projectService.getFee(this.projectId).subscribe((res) => {
      this.fee = res?.data;
      if (this.fee?.fees) {
        this.diagramOptions = {
          ...this.diagramOptions,
          series: Object.keys(this.fee.fees).map((k) => this.fee?.fees[k]),
          labels: Object.keys(this.fee.fees).map((k) => k)
        };
      }
      this.isDataLoaded = true;
    });
  }

}
