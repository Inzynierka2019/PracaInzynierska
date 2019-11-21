import { Component, OnInit, Input } from '@angular/core';
import { AppUnityConnectionStatusService } from 'src/app/services/app-unity-connection-status.service';
import { DataService } from 'src/app/services/data.service';
import { VehiclePopulationData, AvgSpeedStatsData } from 'src/app/interfaces/chart-models';
import { StatsServiceService } from 'src/app/services/stats-service.service';
import { SummaryReport } from 'src/app/interfaces/summary-report';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {
  @Input()
  showSummary: Boolean = false;
  vehiclePopulationData: VehiclePopulationData;
  personalityStats: number[];
  avgSpeedStats: AvgSpeedStatsData;
  summaryReport: SummaryReport[];
  
  constructor(
    private appService: AppUnityConnectionStatusService,
    private dataService: DataService,
    private statsService: StatsServiceService) {}

  ngOnInit() {
    this.vehiclePopulationData = this.dataService.getAllVehiclePopulationData();
    this.personalityStats = this.dataService.getPersonalityStats();
    this.avgSpeedStats = this.dataService.getAvgSpeedStats();
    this.statsService.getSummaryReport().subscribe((report) => this.summaryReport = report);
  }

  show(): void {
    this.showSummary = true;
  }

  get appTime(): string {
    return this.appService.appTimeSpan;
  };

}
