import { Component, OnInit, ViewChild, Input, ElementRef } from '@angular/core';
import { ChartType, ChartOptions } from 'chart.js';
import { HubMethod, StatisticsMethods } from 'src/app/interfaces/utilities';
import { DataService } from 'src/app/services/data.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import * as Chart from 'chart.js';
import { PersonalityStats, AvgSpeedStatsData } from 'src/app/interfaces/chart-models';

@Component({
  selector: 'app-avg-speed',
  templateUrl: './avg-speed.component.html',
  styleUrls: ['./avg-speed.component.css']
})
export class AvgSpeedComponent implements OnInit {
  @ViewChild("chart", { static: true }) chart: ElementRef;
  @Input() title: string;
  @Input() simulationData: AvgSpeedStatsData;

  readonly maxLabelCount = 10;
  public myLineChart: Chart;
  public chartLegend = true;
  public lineChartData: any;
  public chartOptions: any = {
    responsive: true,
    hoverMode: 'index',
    stacked: false,
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
        type: 'linear',
        display: true,
        id: 'y-axis-0',
        beginAtZero: true
      },
      {
        type: 'linear',
        display: false,
        id: 'y-axis-1',
        beginAtZero: true
      },
      {
        type: 'linear',
        display: false,
        id: 'y-axis-2',
        beginAtZero: true,
        gridLines: { // grid line settings
          drawOnChartArea: false, // only want the grid lines for one axis to show up
        },
      }],
    }
  }

  constructor(
    private hub: SignalRService,
    private dataService: DataService) {

    this.lineChartData = {
      labels: [],
      datasets: [
        {
          label: 'slow drivers',
          borderColor: "#f1c40f",
          backgroundColor: "#f1c40f",
          borderWidth: 1.0,
          pointRadius: 1.0,
          pointHoverRadius: 1.0,
          stepped: true,
          fill: false,
          data: [
          ],
          yAxisID: 'y-axis-0'
        },
        {
          label: 'normal drivers',
          borderColor: "#2ecc71",
          backgroundColor: "#2ecc71",
          borderWidth: 1.0,
          pointRadius: 1.0,
          pointHoverRadius: 1.0,
          stepped: true,
          fill: false,
          data: [
          ],
          yAxisID: 'y-axis-1'
        },
        {
          label: 'aggresive drivers',
          borderColor: "#DC143C",
          backgroundColor: "#DC143C",
          borderWidth: 1.0,
          pointRadius: 1.0,
          pointHoverRadius: 1.0,
          stepped: true,
          fill: false,
          data: [
          ],
          yAxisID: 'y-axis-2'
        },
      ]
    };
  }

  ngOnInit() {
    console.log(this.simulationData);
    if (this.simulationData) {
      this.lineChartData.datasets[0].data = this.simulationData.slow;
      this.lineChartData.datasets[1].data = this.simulationData.normal;
      this.lineChartData.datasets[2].data = this.simulationData.aggresive;
      this.lineChartData.labels = this.simulationData.labels;
    }
    else {
      this.ListenForData();
    }

    this.createLineChart(this.lineChartData);
  }

  createLineChart(data) {
    setTimeout(() => {
      this.myLineChart = new Chart(this.chart.nativeElement, {
        type: 'line',
        data: data,
        options: this.chartOptions
      });
    }, 0);
  }

  ListenForData() {
    console.log("listening for data!");
    this.hub.registerHandler(
      StatisticsMethods.get(HubMethod.AvgSpeedByPersonality),
      (stats: PersonalityStats) => {

        this.dataService.addAvgSpeedStats(stats, '');
        let label = new Date().toLocaleTimeString();
        this.myLineChart.data.labels.push(label);
        this.myLineChart.data.datasets[0].data.push(stats.slow);
        this.myLineChart.data.datasets[1].data.push(stats.normal);
        this.myLineChart.data.datasets[2].data.push(stats.aggresive);

        if (this.myLineChart.data.labels.length > this.maxLabelCount) {
          this.myLineChart.data.labels.splice(0, 1);
          this.myLineChart.data.datasets[0].data.splice(0, 1);
          this.myLineChart.data.datasets[1].data.splice(0, 1);
          this.myLineChart.data.datasets[2].data.splice(0, 1);
        }

        this.myLineChart.update();
      });
  }
}