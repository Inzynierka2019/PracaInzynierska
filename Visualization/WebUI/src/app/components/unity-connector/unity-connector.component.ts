import { Component, OnInit } from '@angular/core';
import { UnityService } from '../../services/unity.service';
import { AppUnityConnectionStatusService } from '../../services/app-unity-connection-status.service';

@Component({
  selector: 'app-unity-connector',
  templateUrl: './unity-connector.component.html',
  styleUrls: ['./unity-connector.component.css']
})
export class UnityConnectorComponent implements OnInit {

  isConnected: Boolean = false;
  isLoading: Boolean = false;
  error: Boolean = false;

  constructor(
    private unityApi: UnityService, 
    private appConnection: AppUnityConnectionStatusService) { }

  ngOnInit() {
  }
  
  build() : void{
    this.error = false;
    this.isLoading = true;
    this.unityApi.build().subscribe(
      (data: any) => {
        this.isLoading = false;
      },
      error => this.error = true
    );
  }

  run() : void {
    this.error = false;
    this.isLoading = true;
    this.unityApi.run().subscribe(
      (data: any) => {
        this.isLoading = false;
      },
      error => this.error = true
    );
  }

  getConnectionClass() : string {
    return this.isConnected ? "active" : "inactive";
  }
}
