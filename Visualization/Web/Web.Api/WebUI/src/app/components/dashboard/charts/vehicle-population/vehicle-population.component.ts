import { Component, OnInit } from '@angular/core';
import { SignalRService } from 'src/app/services/signal-r.service';
import { HubMethod, StatisticsMethods } from './../../../../interfaces/utilities';
import { Chart, ChartType } from './../../../../interfaces/chart';
import { VehiclePopulation } from '../../../../interfaces/chart-models';

@Component({
  selector: 'app-vehicle-population',
  templateUrl: './vehicle-population.component.html',
  styleUrls: ['./vehicle-population.component.css']
})
export class VehiclePopulationComponent implements OnInit {

  readonly maxLabelCount = 10;
  defaultColors: any[] = [{ backgroundColor: 'blue' }];
  vehicleChart: Chart;
  dataSet: number[] = new Array();
  labels: string[] = new Array();
  currentPopulation: number;

  constructor(public hub: SignalRService) {
    this.vehicleChart = new Chart(ChartType.Line, new Array(), this.defaultColors);
    
    this.vehicleChart.data = [{
      data: new Array(),
      label: 'vehicle population',
      lineTension: 0.3
    }];
  }

  ngOnInit() {
    this.VehiclePopulation();
  }

  VehiclePopulation(): void {
    this.hub.registerHandler(
      StatisticsMethods.get(HubMethod.VehiclePopulation),
      (vehiclePopulation: VehiclePopulation) => {
        const label = new Date().toLocaleTimeString();
        const count = vehiclePopulation.vehicleCount;

        this.vehicleChart.chartLabels.push(label);
        this.vehicleChart.data[0].data.push(count);
        this.currentPopulation = count;

        if(this.vehicleChart.chartLabels.length > this.maxLabelCount) {
          this.vehicleChart.chartLabels.splice(0,1);
          this.vehicleChart.data[0].data.splice(0,1);
        }
    });
  }
}
