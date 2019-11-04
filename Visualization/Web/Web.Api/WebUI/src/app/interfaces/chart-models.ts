export interface VehiclePopulation {
    timestamp: Date;
    vehicleCount: number;
    vehiclePositions: Array<GeoPosition>;
  }
  
export interface GeoPosition {
    id: number;
    latitude: number;
    longitude: number;
}