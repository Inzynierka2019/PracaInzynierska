import { Injectable } from '@angular/core';
import { ConsoleLog } from '../interfaces/console-log';
import { SignalRService } from './signal-r.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogsService {
  _logs: ConsoleLog[] = [];
  logs$ = new Subject<ConsoleLog[]>();
  hubMethod = "SignalForConsoleLogs";
  
  constructor(private hub: SignalRService) {
    this.hub.registerHandler(this.hubMethod, (data) => {
      var log = new ConsoleLog(data.message, data.timeStamp, data.logType);
      this._logs.push(log);
      this.logs$.next(this._logs);
    });
  }

  public clear(): void { 
    this._logs = [];
  }
}
