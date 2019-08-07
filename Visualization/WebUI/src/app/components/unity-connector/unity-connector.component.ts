import { Component, OnInit } from '@angular/core';
import { UnityService } from '../../services/unity.service';
import { AppUnityConnectionStatusService } from '../../services/app-unity-connection-status.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';

@Component({
  selector: 'app-unity-connector',
  templateUrl: './unity-connector.component.html',
  styleUrls: ['./unity-connector.component.css']
})
export class UnityConnectorComponent implements OnInit {

  isConnected: Boolean = false;
  isRunning: Boolean = false;
  isBuilding: Boolean = false;
  runError: Boolean = false;
  buildError: Boolean = false;

  constructor(
    private unityApi: UnityService, 
    private appConnection: AppUnityConnectionStatusService,
    private snackBar: SnackBarService) { }

  ngOnInit() {
  }
  
  build() : void{
    this.snackBar.open("Build has started!");

    this.buildError = false;
    this.isBuilding = true;
    this.unityApi.build().subscribe(
      (data: any) => {
        this.isBuilding = false;
        this.snackBar.open("Build has finished!");
      },
      error => {
        this.buildError = true;
        this.snackBar.open("Build failed.");
      }
    );
  }

  run() : void {
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

  getConnectionClass() : string {
    return this.isConnected ? "active" : "inactive";
  }
}
