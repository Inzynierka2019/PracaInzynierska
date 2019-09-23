import { Component, OnInit } from '@angular/core';
import { ConsoleColor } from './../../../interfaces/utilities';
import { ConsoleLog } from '../../../interfaces/console-log';
import { LogsService } from 'src/app/services/logs.service';
import * as $ from 'jquery';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {

  logs : ConsoleLog[];
  logSubscription: any; 
  constructor(private logService: LogsService) {
    this.logSubscription = this.logService.logs$.subscribe(
      (logs) => this.logs = logs
    );
  }

  ngOnInit() {
    this.logs = this.logService._logs;
  }

  logsToText(logs: ConsoleLog[]): string {
    return logs.map(log => {
      return log.message;
    }).join('\r\n');
  }

  getLogColorClass(log: ConsoleLog): string {
    if(!log) {
      return '';
    }
    
    return `console-color-${ConsoleColor[log.getConsoleColor()]}`;
  }

  clear() : void {
    this.logs = [];
    this.logService.clear();
  }

  copy() : void {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(this.logsToText(this.logs))).select();
    document.execCommand("copy");
    $temp.remove();
  }
}
