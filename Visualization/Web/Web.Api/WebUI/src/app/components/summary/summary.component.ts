import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {

  showSummary: boolean = true;
  constructor() { }

  ngOnInit() {
  }

  show() {
    this.showSummary = true;
  }
}
