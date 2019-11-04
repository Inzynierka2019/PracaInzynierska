import { Component, OnInit, Input } from '@angular/core';
import { AppUnityConnectionStatusService } from 'src/app/services/app-unity-connection-status.service';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {
  @Input()
  showSummary: Boolean = false;

  constructor(private appService: AppUnityConnectionStatusService) { }

  ngOnInit() {
  }

  show() {
    this.showSummary = true;
  }

  get appTime(): string {
    return this.appService.appTimeSpan;
  };

}
