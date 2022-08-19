import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogBase } from 'mosaico-base';
import { getCSSVariableValue } from 'src/app/_metronic/kt/_utils';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-vesting-token-dynamic',
  templateUrl: './vesting-token-dynamic.component.html',
  styleUrls: ['./vesting-token-dynamic.component.scss']
})
export class VestingTokenDynamicComponent extends DialogBase implements OnInit, OnDestroy, AfterViewInit {
  sub: SubSink = new SubSink();
  chartColor: string = 'primary';
  chartHeight: string = '150px';
  chartOptions: any = {};
  constructor(modalService: NgbModal) {
    super(modalService);
  }

  ngAfterViewInit(): void {

  }

  ngOnDestroy(): void {

  }

  ngOnInit(): void {
    this.chartOptions = this.getChartOptions(this.chartHeight, this.chartColor);
  }

  getChartOptions(chartHeight: string, chartColor: string) {
    const labelColor = getCSSVariableValue('--bs-gray-800');
    const strokeColor = getCSSVariableValue('--bs-gray-300');
    const baseColor = getCSSVariableValue('--bs-' + chartColor);
    const lightColor = getCSSVariableValue('--bs-light-' + chartColor);
  
    return {
      series: [
        {
          name: 'Net Profit',
          data: [15, 25, 15, 40, 20, 50],
        },
      ],
      chart: {
        fontFamily: 'inherit',
        type: 'area',
        height: chartHeight,
        toolbar: {
          show: false,
        },
        zoom: {
          enabled: false,
        },
        sparkline: {
          enabled: true,
        },
      },
      plotOptions: {},
      legend: {
        show: false,
      },
      dataLabels: {
        enabled: false,
      },
      fill: {
        type: 'solid',
        opacity: 1,
      },
      stroke: {
        curve: 'smooth',
        show: true,
        width: 3,
        colors: [baseColor],
      },
      xaxis: {
        categories: ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
        labels: {
          show: false,
          style: {
            colors: labelColor,
            fontSize: '12px',
          },
        },
        crosshairs: {
          show: false,
          position: 'front',
          stroke: {
            color: strokeColor,
            width: 1,
            dashArray: 3,
          },
        },
        tooltip: {
          enabled: false,
        },
      },
      yaxis: {
        min: 0,
        max: 60,
        labels: {
          show: false,
          style: {
            colors: labelColor,
            fontSize: '12px',
          },
        },
      },
      states: {
        normal: {
          filter: {
            type: 'none',
            value: 0,
          },
        },
        hover: {
          filter: {
            type: 'none',
            value: 0,
          },
        },
        active: {
          allowMultipleDataPointsSelection: false,
          filter: {
            type: 'none',
            value: 0,
          },
        },
      },
      tooltip: {
        style: {
          fontSize: '12px',
        },
        y: {
          formatter: function (val: number) {
            return '$' + val + ' thousands';
          },
        },
      },
      colors: [lightColor],
      markers: {
        colors: [lightColor],
        strokeColors: [baseColor],
        strokeWidth: 3,
      },
    };
  }

}
