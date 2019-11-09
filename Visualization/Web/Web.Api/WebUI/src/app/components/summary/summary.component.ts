import { Component, OnInit, Input } from '@angular/core';
import { AppUnityConnectionStatusService } from 'src/app/services/app-unity-connection-status.service';
import { DataService } from 'src/app/services/data.service';
import { VehiclePopulationData } from 'src/app/interfaces/chart-models';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {
  @Input()
  showSummary: Boolean = false;
  vehiclePopulationData: VehiclePopulationData;

  constructor(
    private appService: AppUnityConnectionStatusService,
    private dataService: DataService) { }

  ngOnInit() {
    this.vehiclePopulationData = this.dataService.getAllVehiclePopulationData();
  }

  show(): void {
    this.showSummary = true;
  }

  refresh(): void {
    window.location.reload();
  }

  get appTime(): string {
    return this.appService.appTimeSpan;
  };

}
