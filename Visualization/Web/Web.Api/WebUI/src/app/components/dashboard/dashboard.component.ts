import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppUnityConnectionStatusService } from 'src/app/services/app-unity-connection-status.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  errorMsg = "could not connect :(";
  connectionStatus: any;
  showSummary: boolean = true;

  constructor(
    private appStatus: AppUnityConnectionStatusService,
    private spinner: NgxSpinnerService) { 
  }

  get connected(): Boolean {
    return this.appStatus.isConnected;
  }

  ngOnInit() {
    this.spinner.show();
    this.connectionStatus = this.appStatus.status$.subscribe(connection => {
      if(connection) {
        this.spinner.hide();
        /* TODO ??? */
        // this.connectionStatus.unsubscribe();
      } else {
        this.showSummary = true;
      }
    });
    console.log(this.connectionStatus);
  }

  ngOnDestroy() {
    this.connectionStatus.unsubscribe();
  }

  showSpinner() {
    this.spinner.show();
    setTimeout(() => {
      this.spinner.hide();
    }, 5000);
  }
}