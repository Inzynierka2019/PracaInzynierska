import { Component, OnInit } from '@angular/core';
import { UnityService } from '../../services/unity.service';
import { AppUnityConnectionStatusService } from '../../services/app-unity-connection-status.service';

@Component({
  selector: 'app-unity-connector',
  templateUrl: './unity-connector.component.html',
  styleUrls: ['./unity-connector.component.css']
})
export class UnityConnectorComponent implements OnInit {

  isConnected: Boolean;

  constructor(
    private unityApi: UnityService, 
    private appConnection: AppUnityConnectionStatusService) {
    
      this.isConnected = false;
  }

  ngOnInit() {
    // this.appConnection.
  }
  
  build() : void{
    this.unityApi.build();
  }

  run() : void {
    this.unityApi.run();
  }

  getConnectionClass() : string {
    return this.isConnected ? "active" : "inactive";
  }

}
