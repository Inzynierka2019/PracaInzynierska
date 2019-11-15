import { Component, OnInit, Input } from '@angular/core';
import { SummaryReport } from 'src/app/interfaces/summary-report';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  @Input() summaryReport: SummaryReport[];
  constructor() { }

  ngOnInit() {
    console.log(this.summaryReport);
  }

}
