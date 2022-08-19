import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { ProjectService } from 'mosaico-project';
import { ApexAxisChartSeries, ApexChart, ApexDataLabels, ApexLegend, ApexNonAxisChartSeries, ApexPlotOptions, ApexResponsive, ApexTheme, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { SubSink } from 'subsink';
import { selectProjectPreview, selectProjectPreviewToken, selectProjectPage } from '../../store/project.selectors';
import { Token } from 'mosaico-wallet';

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: string[];
  legend: ApexLegend;
  plotOptions: ApexPlotOptions;
  theme: ApexTheme;
  dataLabels: ApexDataLabels;
};

@Component({
  selector: 'app-project-tokenomics',
  templateUrl: './project-tokenomics.component.html',
  styleUrls: ['./project-tokenomics.component.scss']
})
export class ProjectTokenomicsComponent implements OnInit, OnDestroy{
  @ViewChild("chart") chart: ChartComponent;
  chartOptions: Partial<ChartOptions>;

  private numberOfLabels = 0;
  private series: number[] = [];
  private labels: string[] = [];

  private primaryColor = '';

  isLoading = false;
  hasData = false;

  private subs = new SubSink();

  private projectId = '';;
  private token: Token;
  private percentages: number[] = [];

  constructor(
    private projectService: ProjectService,
    private store: Store
  ) {
    this.chartOptions = {
      chart: {
        type: "donut",
        width: 470,
        height: '100%',
      },
      theme: {
        monochrome: {
          enabled: true,
          color: '#494642'
        }
      },
      legend: {
        position: "right",
        horizontalAlign: "left",
        formatter: this.legendLabelFormatter.bind(this),
        // height: 600
      },
      dataLabels: {
        enabled: true,
        formatter: this.dataLabelFormatter.bind(this),
        style: {
          fontFamily: "Poppins",
          fontWeight: 'normal'
        },
        dropShadow: {
          enabled: true
        }
      },
      responsive: [
        {
          breakpoint: 576,
          options: {
            chart: {
              width: '100%',
              height: 260
            },
            legend: {
              position: "bottom",
              horizontalAlign: "center",
              height: "100%",
              itemMargin: {
                vertical: 4,
              }
            }
          }
        }
      ]
    };
  }

  ngOnInit(): void {
    this.getProjectIdAndToken();
    this.getColorPrimary();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private getProjectIdAndToken(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe((res) => {
      if (res && res.project) {
        this.projectId = res.project.id;
        this.token = res.token;
        this.getDataTokenomics();
      }
    });
  }

  private getColorPrimary(): void {
    this.subs.sink = this.store.select(selectProjectPage).subscribe((page) => {
      const currentColor = this.primaryColor;
      if(page?.primaryColor && page?.primaryColor.length > 0) {
        this.primaryColor = page?.primaryColor;

      } else {
        this.primaryColor = '#494642';
      }

      if (currentColor !== '' && currentColor !== this.primaryColor) {
        this.reloadTokenomics(true);
      }
    });
  }


  private getDataTokenomics(force: boolean = false): void {
    if (this.projectId && this.hasData === false && this.isLoading === false) {
      this.isLoading = true;
      this.subs.sink = this.projectService.getTokenomics(this.projectId).subscribe((res) => {
        if (res && res.data) {
          this.series = res.data.series;
          this.numberOfLabels = res.data.series && res.data.series.length;
          this.labels = res.data.labels;
          this.percentages = res.data.percentage;
          this.hasData = this.series && this.series.length > 0;
          this.reloadTokenomics();
          this.isLoading = false;
        }
      }, (error) => { this.isLoading = false; });
    }
  }

  private reloadTokenomics(addDelay = false): void {
    this.isLoading = true;
    if (this.numberOfLabels > 0) {
      this.chartOptions.chart.width = this.getDimensionsChartDesktop(this.numberOfLabels);
      this.chartOptions.chart.height = this.getDimensionsChartDesktop(this.numberOfLabels);
      this.chartOptions.responsive[0].options.chart.height = this.getDimensionsChartMobile(this.numberOfLabels);
    }
    if (this.primaryColor) {
      this.chartOptions.theme.monochrome.color = this.primaryColor;
    }
    this.chartOptions = {
      ...this.chartOptions,
      labels: this.labels,
      series: this.series
    };
    if (addDelay === false) {
      this.isLoading = false;
    } else if (addDelay === true) {
      setTimeout(() => {
        this.isLoading = false;
      }, 500);

      // this.chart.render();
    }

  }

  private getDimensionsChartDesktop(numberOfLabels: number): number {
    if (numberOfLabels <= 7) {
      return 470;
    } else {
      return (470 + ((numberOfLabels - 7) * 29));
    }
  }

  private getDimensionsChartMobile(numberOfLabels: number): number {
    if (numberOfLabels === 1) {
      return 260;
    } else {
      return (260 + ((numberOfLabels - 1) * 21)); // 21 height px - 1 label
    }
  }

  private dataLabelFormatter(label: string, opts: any): string {
    const percentage = this.percentages[opts?.seriesIndex];
    return `${percentage?.toFixed(2)} %`;
  }

  private legendLabelFormatter(label: string, opts: any): string {
    return `${label} (${opts?.w?.globals?.series[opts?.seriesIndex]} ${this.token?.symbol})`;
  }

}
