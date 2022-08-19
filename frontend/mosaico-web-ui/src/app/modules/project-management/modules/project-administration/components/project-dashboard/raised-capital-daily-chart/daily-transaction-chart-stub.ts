import {ApexOptions} from "ng-apexcharts/lib/model/apex-types";

export {baseConfiguration};

const baseConfiguration: ApexOptions = {
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
    labels: {
      formatter: function (val) {
        return (val / 1000000).toFixed(0);
      }
    },
    title: {
      text: "Price"
    },
  },
  xaxis: {
    type: "datetime"
  }
};

