import { Component, OnInit, Input } from '@angular/core';
import { SignalRService } from 'src/app/services/signal-r.service';
import { HubMethod, StatisticsMethods } from './../../../../interfaces/utilities';
import { Chart, ChartType } from './../../../../interfaces/chart';
import { VehiclePopulation, VehiclePopulationData } from '../../../../interfaces/chart-models';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-vehicle-population',
  templateUrl: './vehicle-population.component.html',
  styleUrls: ['./vehicle-population.component.css']
})
export class VehiclePopulationComponent implements OnInit {
  @Input() data: VehiclePopulationData;

  readonly maxLabelCount = 10;
  defaultColors: any[] = [{ backgroundColor: 'blue' }];
  vehicleChart: Chart;
  dataSet: number[] = new Array();
  labels: string[] = new Array();
  currentPopulation: number;

  constructor(
    private hub: SignalRService,
    private dataService: DataService) {
    this.vehicleChart = new Chart(ChartType.Line, new Array(), this.defaultColors);

    this.vehicleChart.data = [{
      data: new Array(),
      label: 'vehicle population',
      lineTension: 0.3
    }];
  }

  ngOnInit() {
    if (this.data == null) {
      this.ListenForData();
    }
    else {
      console.log(this.data);
      console.log(this.vehicleChart);
      this.vehicleChart.data = [{
        data: this.data.data,
        label: 'vehicle population',
        lineTension: 0.3
      }];
      this.vehicleChart.chartLabels = this.data.labels;
    }
  }

  ListenForData(): void {
    this.hub.registerHandler(
      StatisticsMethods.get(HubMethod.VehiclePopulation),
      (vehiclePopulation: VehiclePopulation) => {
        let count = vehiclePopulation.vehicleCount;
        let label = new Date().toLocaleTimeString();

        this.dataService.addVehicleData(count, '');

        this.vehicleChart.chartLabels.push(label);
        this.vehicleChart.data[0].data.push(count);
        this.currentPopulation = count;

        if (this.vehicleChart.chartLabels.length > this.maxLabelCount) {
          this.vehicleChart.chartLabels.splice(0, 1);
          this.vehicleChart.data[0].data.splice(0, 1);
        }
      });
  }
}
