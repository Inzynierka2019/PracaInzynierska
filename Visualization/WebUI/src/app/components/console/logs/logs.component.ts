import { Component, OnInit, Input } from '@angular/core';
import { ConsoleColor } from '../../../interfaces/log-message-type.enum';
import { ConsoleLog } from '../../../interfaces/console-log';
import * as $ from 'jquery';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {

  @Input() logs : ConsoleLog[] = [];

  constructor() { }

  ngOnInit() {
    let log = new ConsoleLog();
    log.message = "this is example log";
    log.timeStamp = "15:23:33";
    log.logType = 0;

    for(var i = 0; i < 50; i++) {
      this.logs.push(log);
    }
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
    console.log("console-color-${ConsoleColor[log.getConsoleColor()]}");
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
