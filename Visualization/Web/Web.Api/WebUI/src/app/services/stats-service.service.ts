import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SummaryReport } from '../interfaces/summary-report';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatsServiceService {
  url = '/api/stats';

  constructor(private http: HttpClient) { }

  getSummaryReport(): Observable<SummaryReport[]> {
    return this.http.get<SummaryReport[]>(`${this.url}/summary`);
  }
}
