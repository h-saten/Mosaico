import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-transactions-chart',
  templateUrl: './transactions-chart.component.html',
  styleUrls: []
})
export class TransactionsChartComponent implements OnInit {


  transactionsStatistics: any;

  constructor(
  ) {
    this.transactionsStatistics = {
      series: [
        {
          name: "Paid",
          data: [44, 55, 41, 67, 22, 43]
        },
        {
          name: "Waiting",
          data: [13, 23, 20, 8, 13, 27]
        },
        {
          name: "Cancelled",
          data: [11, 17, 15, 15, 21, 14]
        },
        {
          name: "Expired",
          data: [21, 7, 25, 13, 22, 8]
        }
      ],
      theme: {
        monochrome: {
          enabled: true,
          color: '#858381'
        }
      },
      chart: {
        type: "bar",
        height: 350,
        stacked: true,
        toolbar: {
          show: true
        },
        zoom: {
          enabled: true
        }
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            legend: {
              position: "bottom",
              offsetX: -10,
              offsetY: 0
            }
          }
        }
      ],
      plotOptions: {
        bar: {
          horizontal: false
        }
      },
      xaxis: {
        type: "category",
        categories: [
          "01/2011",
          "02/2011",
          "03/2011",
          "04/2011",
          "05/2011",
          "06/2011"
        ]
      },
      legend: {
        position: "right",
        offsetY: 40
      },
      fill: {
        opacity: 1
      }
    };
  }

  ngOnInit(): void {
  }
}
