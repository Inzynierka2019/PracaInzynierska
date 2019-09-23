import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SignalRService } from 'src/app/services/signal-r.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  connected = false;
  connectionError = false;
  timeout = 3000;
  isLoading = false;
  loaderMsg = "waiting for unity app...";
  errorMsg = "could not connect :(";

  constructor(
    public hub: SignalRService,
    private spinner: NgxSpinnerService) { 
     
  }

  ngOnInit() {
    this.spinner.show();
    this.asyncInit();
  }

  async asyncInit() {
    await this.waitForConnection().then(() => {
      this.connected = true;
      this.spinner.hide();
    });
  }

  waitForConnection() {
    return new Promise(resolve => {
      setTimeout(() => {
        resolve();
      }, 3000);
    });
  }
}