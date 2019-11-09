import { Component, OnInit } from '@angular/core';
import { UnityService } from '../../services/unity.service';
import { AppUnityConnectionStatusService } from '../../services/app-unity-connection-status.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-unity-connector',
  templateUrl: './unity-connector.component.html',
  styleUrls: ['./unity-connector.component.css']
})
export class UnityConnectorComponent implements OnInit {

  isRunning: Boolean = false;
  isBuilding: Boolean = false;
  runError: Boolean = false;
  buildError: Boolean = false;
  connectionStatus: any;

  constructor(
    private unityApi: UnityService,
    private appStatus: AppUnityConnectionStatusService,
    private snackBar: SnackBarService) { }

  ngOnInit() {
  }
  
  runWebGl() : void{
    this.snackBar.open("Simulation opened in new window!");
    window.open("//unity/Simulation.exe/index.html", "_blank");
  }

  runWindows() : void {
    this.runError = false;
    this.isRunning = true;
    this.unityApi.run().subscribe(
      (data: any) => {
        this.isRunning = false;
      },
      error => {
        this.runError = true;
        this.snackBar.open("Could not run Unity App.");
      }
    );
  }

  getAppStatusClass() : string {
    return this.appStatus.isConnected ? "connected" : "disconnected";
  }
}
