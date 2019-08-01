import { Component, OnInit, Input } from '@angular/core';
import { ConsoleColor, LogType } from '../../../interfaces/log-message-type.enum';
import { ConsoleLog } from '../../../interfaces/console-log';
import { SignalRService } from 'src/app/services/signal-r.service';
import * as $ from 'jquery';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {

  logs : ConsoleLog[] = [];

  hubMethod = "SignalForConsoleLogs";

  constructor(private hub: SignalRService) { }

  ngOnInit() {
    this.logs = [];

    this.hub.registerHandler(this.hubMethod, (data) => {
      var log = new ConsoleLog(data.message, data.timeStamp, data.logType);
      this.logs.push(log);
    });
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
  }

  copy() : void {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(this.logsToText(this.logs))).select();
    document.execCommand("copy");
    $temp.remove();
  }
}
