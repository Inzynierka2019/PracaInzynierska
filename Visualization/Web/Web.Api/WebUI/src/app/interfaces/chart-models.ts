export interface VehiclePopulation {
  timestamp: Date;
  vehicleCount: number;
  vehiclePositions: Array<GeoPosition>;
}

export class VehiclePopulationData {
  data: number[];
  labels: string[];
  constructor() {
    this.data = new Array<number>();
    this.labels =  new Array<string>();
  }
}

export interface GeoPosition {
  id: number;
  latitude: number;
  longitude: number;
}