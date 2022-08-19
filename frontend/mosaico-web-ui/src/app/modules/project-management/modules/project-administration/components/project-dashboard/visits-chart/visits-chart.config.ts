
export {baseConfiguration};

export interface ChartSeries {
  name: string;
  data: string[];
}

const baseConfiguration = {
  series: [] as ChartSeries[],
    theme: {
      monochrome: {
        enabled: true,
        color: '#858381'
      }
  },
  chart: {
    height: 350,
      type: "line",
      zoom: {
      enabled: false
    }
  },
  dataLabels: {
    enabled: false
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
    categories: []
  }
};
