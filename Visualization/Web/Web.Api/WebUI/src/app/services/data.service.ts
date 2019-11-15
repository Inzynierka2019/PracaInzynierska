import { Injectable } from '@angular/core';
import { VehiclePopulationData, DriverReport, PersonalityStats, AvgSpeedStatsData } from '../interfaces/chart-models';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public vehiclePopulationData: VehiclePopulationData;
  public driverReports: any;
  public personalityStats: PersonalityStats;
  public avgSpeedStatsData: AvgSpeedStatsData;
  
  constructor() {
    this.vehiclePopulationData = new VehiclePopulationData();
    this.personalityStats = new PersonalityStats();
    this.avgSpeedStatsData = new AvgSpeedStatsData();
  }

  addVehicleData(count: number, label: string) {
    this.vehiclePopulationData.data.push(count);
    this.vehiclePopulationData.labels.push(label);
  }
  
  getAllVehiclePopulationData(): VehiclePopulationData {
    return this.vehiclePopulationData;
  }

  addDriverReport(report: DriverReport) {
    this.driverReports.push(report);
  }

  getAllDriverReports() : DriverReport[] {
    return this.driverReports;
  }

  addPersonalityStats(stats: PersonalityStats) : void {
    this.personalityStats.aggresive += stats.aggresive;
    this.personalityStats.normal += stats.normal;
    this.personalityStats.slow += stats.slow;
    this.personalityStats.count++;
  }

  addAvgSpeedStats(stats: PersonalityStats, label: string) : void {
    this.avgSpeedStatsData.slow.push(stats.slow);
    this.avgSpeedStatsData.normal.push(stats.normal);
    this.avgSpeedStatsData.aggresive.push(stats.aggresive);
    this.avgSpeedStatsData.labels.push(label);
  }

  getPersonalityStats() : number[] {
    console.log(this.personalityStats)

    return [ // average
      this.personalityStats.slow / this.personalityStats.count, 
      this.personalityStats.normal / this.personalityStats.count,
      this.personalityStats.aggresive / this.personalityStats.count
    ];
  }

  getAvgSpeedStats() : AvgSpeedStatsData {
    return this.avgSpeedStatsData;
  }
}
