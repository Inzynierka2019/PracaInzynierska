import { Injectable } from '@angular/core';
import { SignalRService } from './signal-r.service';
import { Observable, Subject } from 'rxjs';
import { VehicleStatus, VehiclePopulation } from '../interfaces/chart-models';

@Injectable({
  providedIn: 'root'
})
export class VehiclePositionsService {

    public addressPoints: Array<VehicleStatus>;
    private pointsObserver$: Subject<Array<VehicleStatus>>;

  constructor(private signalR: SignalRService) {
    this.registerSignalHandlers();
      this.addressPoints = new Array<VehicleStatus>();
      this.pointsObserver$ = new Subject<Array<VehicleStatus>>();
  }

  registerSignalHandlers(): void {    
    this.signalR.registerHandler(
      'SignalForVehiclePopulation',
      (population: VehiclePopulation) => {
        this.addressPoints = population.vehicleStatuses;
        this.pointsObserver$.next(this.addressPoints);
      });
  }

    getAddressPoints(): Observable<Array<VehicleStatus>> {
    return this.pointsObserver$.asObservable();
  }
}