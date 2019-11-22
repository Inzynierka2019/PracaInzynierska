import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SimulationPreferences } from '../interfaces/scene-preferences';
import { GeoPosition } from '../interfaces/chart-models';

@Injectable({
  providedIn: 'root'
})
export class UnityService {
  url = '/api/unity';

  constructor(private httpClient: HttpClient) { }

  build() {
    return this.httpClient.get(this.url + '/build');
  }

  run() {
    return this.httpClient.get(this.url + '/run');
  }

  getPreferences() : Observable<SimulationPreferences> {
    return this.httpClient.get<SimulationPreferences>(`${this.url}/preferences`);
  }

  savePreferences(preferences: SimulationPreferences) : Observable<SimulationPreferences> {
    return this.httpClient.put<SimulationPreferences>(`${this.url}/preferences`, preferences);
  }

  getGeoPositionReference() : Observable<GeoPosition> {
    return this.httpClient.get<GeoPosition>(`${this.url}/geoposition`);
  }

  handleError(error) {
    let errorMessage = '';
    if(error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return errorMessage;
  }
}
