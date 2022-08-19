import {ApexOptions} from "ng-apexcharts";

export {baseConfiguration};

export interface ChartSeries {
  name: string;
  data: string[];
}

const baseConfiguration: ApexOptions = {
  series: [],
  theme: {
      monochrome: {
        enabled: true,
        color: '#858381'
      }
  },
  chart: {
    type: "area",
    stacked: false,
    height: 350,
    zoom: {
      type: "x",
      enabled: false,
      autoScaleYaxis: true
    },
    toolbar: {
      autoSelected: "zoom"
    }
  },
  dataLabels: {
    enabled: false
  },
  markers: {
    size: 0
  },
  fill: {
    type: "gradient",
    gradient: {
      shadeIntensity: 1,
      inverseColors: false,
      opacityFrom: 0.5,
      opacityTo: 0,
      stops: [0, 90, 100]
    }
  },
  yaxis: {
    title: {
      text: ""
    }
  },
  stroke: {
    curve: "straight"
  },
  grid: {
    row: {
      colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
        opacity: 0.5
    }
  },
  xaxis: {
    type: "datetime"
  }
};
