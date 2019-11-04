import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SimulationPreferences } from '../interfaces/simulation-preferences';
import { Observable } from 'rxjs';
import { ScenePreferences } from '../interfaces/scene-preferences';

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

  getPreferences() : Observable<ScenePreferences[]> {
    return this.httpClient.get<ScenePreferences[]>(`${this.url}/preferences`);
  }

  savePreferences(preferences: ScenePreferences[]) : Observable<ScenePreferences[]> {
    return this.httpClient.post<ScenePreferences[]>(`${this.url}/preferences`, preferences);
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
