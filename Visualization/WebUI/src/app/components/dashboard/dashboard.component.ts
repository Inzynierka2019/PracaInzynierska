import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  connected = false;
  timeout = 1500;
  loader ="waiting for unity app...";

  constructor(private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.spinner.show();
    setTimeout(() => {
      this.spinner.hide();
      this.connected = true;
    }, this.timeout);
  }
}
