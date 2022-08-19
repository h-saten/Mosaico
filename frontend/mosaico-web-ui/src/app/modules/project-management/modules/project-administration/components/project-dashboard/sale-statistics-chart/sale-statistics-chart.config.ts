
export {baseConfiguration};

export interface ChartSeries {
  name: string;
  data: string[];
}

const baseConfiguration = {
  series: [],
  chart: {
    type: "pie",
    background: 'none',
    height: "300px",
  },
  theme: {
    monochrome: {
      enabled: true,
      color: '#858381'
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
  labels: [],
  legend: {
    position: "bottom"
  },
  grid: {
    row: {
      colors: ["#f3f3f3", "transparent"],
      opacity: 0.5
    }
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
