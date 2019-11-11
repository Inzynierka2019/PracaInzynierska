import { Injectable } from '@angular/core';
import { VehiclePopulationData } from '../interfaces/chart-models';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public vehiclePopulationData: VehiclePopulationData;
  
  constructor() {
    this.vehiclePopulationData = new VehiclePopulationData();
  }

  addVehicleData(count: number, label: string) {
    this.vehiclePopulationData.data.push(count);
    this.vehiclePopulationData.labels.push(label);
  }
  
  getAllVehiclePopulationData(): VehiclePopulationData {
    return this.vehiclePopulationData;
  }
}
