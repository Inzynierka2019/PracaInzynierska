import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

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
  
  constructor(private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.isLoading = true;
    this.spinner.show();
    setTimeout(() => {
      this.spinner.hide();
      this.isLoading = false;
      this.connectionError = true;
      // this.connected = true;
    }, this.timeout);
  }
}
