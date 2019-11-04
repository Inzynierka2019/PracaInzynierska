import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { retry, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UnityService {
  url = 'http://localhost:5000/api/unity';

  //TODO Zamienić to na BASE_URL z providera!!

  constructor(private httpClient: HttpClient) { }

  build() {
    return this.httpClient.get(this.url + '/build');
  }

  run() {
    return this.httpClient.get(this.url + '/run');
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
