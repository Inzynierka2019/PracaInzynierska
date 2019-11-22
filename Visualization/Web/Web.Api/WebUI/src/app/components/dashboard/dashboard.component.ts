import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppUnityConnectionStatusService } from 'src/app/services/app-unity-connection-status.service';
import { UnityAppState } from 'src/app/interfaces/unity-app-state';
import { UnityService } from 'src/app/services/unity.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  constructor(
    private appStatus: AppUnityConnectionStatusService,
    private spinner: NgxSpinnerService) {
  }

  get connected(): Boolean {
      switch(this.appStatus.appState) {
      case UnityAppState.CONNECTED:
      case UnityAppState.DISCONNECTED:
      case UnityAppState.RUNNING:
      this.spinner.hide();
      return true;
      case UnityAppState.NOT_CONNECTED:
      this.spinner.show();
      return false;
    }
  }

  get appTime(): string {
    return this.appStatus.appTimeSpan;
  }

  get showSummary(): Boolean {
    return this.appStatus.appState == UnityAppState.DISCONNECTED;
  }

  ngOnInit() {
  }
}