import { Injectable } from '@angular/core';
import { SignalRService } from './signal-r.service';

@Injectable({
  providedIn: 'root'
})
export class AppUnityConnectionStatusService {

  // isConnected: boolean;

  // constructor(private signalR: SignalRService) {
  //   this.isConnected = false;
  //   this.registerSignalHandlers();
  //  }

  // registerSignalHandlers(): void {
  //   this.signalR.registerHandler(
  //     'CheckUnityAppConnectionStatus',
  //     (isConnected: boolean) => {
  //       this.isConnected = isConnected;
  //     }
  //   );
  // }
  
}
