import { Injectable } from '@angular/core';
import { SignalRService } from './signal-r.service';
import { SnackBarService } from './snack-bar.service';

@Injectable({
  providedIn: 'root'
})
export class AppUnityConnectionStatusService {

  public isConnected: boolean;

  constructor(
    private signalR: SignalRService,
    private snackBar: SnackBarService) {
    this.isConnected = false;
    this.registerSignalHandlers();
    this.snackBar.open("Simulator app is disconnected.");
   }

  registerSignalHandlers(): void {
    this.signalR.registerHandler(
      'SignalForUnityAppConnectionStatus',
      (isConnected: boolean) => {
        this.isConnected = isConnected;
      }
    );
  }
  
}
