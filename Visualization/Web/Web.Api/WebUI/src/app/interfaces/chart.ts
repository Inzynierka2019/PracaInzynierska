export enum ChartType {
    Line,
    Bar,
    Pie,
}

export const ChartTypes: Map<ChartType, string> = new Map<ChartType, string>([
    [ChartType.Line, "line"],
    [ChartType.Bar, "bar"],
    [ChartType.Pie, "pie"],
]);

export class Chart {
    public data: any[];
    public chartLabels: string[];
    public chartType: string;
    public chartLegend: boolean;
    public colors: any[];

    constructor(type: ChartType, labels: string[], colors: any[], legend: boolean = true) {
        this.chartType = ChartTypes.get(type);
        this.chartLabels = labels;
        this.chartLegend = legend;
        this.colors = colors; 
    }

    public chartOptions: any = {
        scaleShowVerticalLines: true,
        responsive: true,
        animation: {duration: 0},
        layout: {
            padding: {
                left: 0,
                right: 0,
                top: 0,
                bottom: 0
            }
        },
        scales: {
          xAxes: [{
            ticks: {
              display: true,
              autoSkip: false,
              beginAtZero: false
            }
          }],
          yAxes: [{
            ticks: {
              beginAtZero: true
            }
          }]
        }
      };
}
