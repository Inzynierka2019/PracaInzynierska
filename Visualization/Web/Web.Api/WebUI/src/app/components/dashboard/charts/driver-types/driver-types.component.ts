import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { ChartType, ChartOptions } from 'chart.js';
import { SingleDataSet, Label, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';
import { HubMethod, StatisticsMethods } from 'src/app/interfaces/utilities';
import { DataService } from 'src/app/services/data.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { Personality, PersonalityStats } from 'src/app/interfaces/chart-models';
import * as Chart from 'chart.js';

@Component({
  selector: 'app-driver-types',
  templateUrl: './driver-types.component.html',
  styleUrls: ['./driver-types.component.css']
})
export class DriverTypesComponent implements OnInit {
  @ViewChild("chart", { static: true }) chart: ElementRef;
  @Input() title: string;
  @Input() simulationData: number[];

  myPieChart: Chart;
  public pieChartOptions: ChartOptions = {
    responsive: true,
  };

  public pieChartLegend = true;
  public pieChartData: any;

  constructor(
    private hub: SignalRService,
    private dataService: DataService) {
    monkeyPatchChartJsTooltip();
    monkeyPatchChartJsLegend();
  }

  ngOnInit() {
    if (!this.simulationData) {
      this.simulationData = [1, 1, 1];
      this.ListenForData();
    }
    else {
      console.log(this.simulationData);
    }

    this.pieChartData = {
      labels: ["Slow drivers", "Normal drivers", "Aggresive drivers"],
      datasets: [{
        backgroundColor: [
          "#f1c40f",
          "#2ecc71",
          "#DC143C",
        ],
        data: this.simulationData
      }]
    };

    setTimeout(() => {
      this.myPieChart = new Chart(this.chart.nativeElement, {
        type: 'pie',
        data: this.pieChartData,
        options: this.pieChartOptions
      })
    }, 0);
  }

  ListenForData() {
    this.hub.registerHandler(
      StatisticsMethods.get(HubMethod.PersonalityStats),
      (stats: PersonalityStats) => {
        this.myPieChart.data.datasets[0].data[0] = stats.slow;
        this.myPieChart.data.datasets[0].data[1] = stats.normal;
        this.myPieChart.data.datasets[0].data[2] = stats.aggresive;

        this.dataService.addPersonalityStats(stats);
        this.myPieChart.update();
      });
  }
}