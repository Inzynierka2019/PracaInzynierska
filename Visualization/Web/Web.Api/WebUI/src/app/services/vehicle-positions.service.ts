import { Injectable } from '@angular/core';
import { SignalRService } from './signal-r.service';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VehiclePositionsService {

  public addressPoints: Array<GeoPosition>;
  private pointsObserver$: Subject<Array<GeoPosition>>;

  constructor(private signalR: SignalRService) {
    this.registerSignalHandlers();
    this.addressPoints = new Array<GeoPosition>();
    this.pointsObserver$ = new Subject<Array<GeoPosition>>();
  }

  registerSignalHandlers(): void {    
    this.signalR.registerHandler(
      'SignalForVehiclePopulation',
      (population) => {
        this.addressPoints = population.vehiclePositions;
        this.pointsObserver$.next(this.addressPoints);
      });
  }

  getAddressPoints(): Observable<Array<GeoPosition>> {
    return this.pointsObserver$.asObservable();
  }
}

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

